using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SquidLogAnalyzer
{
    static class HelperMethods
    {
        // Credit due: http://stackoverflow.com/questions/249760/how-to-convert-unix-timestamp-to-datetime-and-vice-versa
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static void checkConfiguration(string[] args)
        {
            // Make sure there are arguments
            if (args.Length == 0)
            {
                Options.ShowUsage();
            }
            // Parse paramaters 
            try
            {
                foreach (string argument in args)
                {
                    if (argument.Contains("-l") == true || argument.Contains("--listen") == true)
                    {
                        Options.listenMode = Int32.Parse(argument.Split('=')[1]);
                    }

                    if (argument.Contains("-d") == true || argument.Contains("--daemon") == true)
                    {
                        Options.daemonMode = true;
                    }

                    if (argument.Contains("-c") == true || argument.Contains("--config") == true)
                    {
                        Options.configrationFile = argument.Split('=')[1];
                    }

                    if (argument.Contains("-g") == true || argument.Contains("--generate") == true)
                    {
                        DateTime temp;
                        if (DateTime.TryParse(argument.Split('=')[1], out temp))
                        {
                            Options.generate = argument.Split('=')[1];
                        }
                        else
                        {
                            // invalid date input
                            Console.WriteLine("Erroneous date input format");
                            Console.ReadKey();
                        }
                    }

                    if (argument.Contains("-s") == true || argument.Contains("--slurp") == true)
                    {
                        Options.slurp = argument.Split('=')[1];
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: Malformed parameter input - " + ex.Message);
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        public static void addLog(string[] logs)
        {
            // Create the object here to save processing time!
            MysqlFactory dbCon = new MysqlFactory();

            int line = 1;

            foreach (String str in logs)
            {
                // Sanitize the data firstly - squid inserts multiple spaces in the first column *ack*
                var temp = Regex.Replace(str, @"\s+", " ").Split(' ');

                if (temp.Count() != 10)
                {
                    Console.WriteLine("ERROR: Malformed line in the proxy log - please see http://wiki.squid-cache.org/Features/LogFormat‎ for proper format on line " + line);
                    break;
                }
                else
                {
                    LogRecord record = new LogRecord();
                    // convert unix timestamp to native DateTime type and then convert to string in format appropriate for mysql server DateTime data type.
                    record.dateStamp = HelperMethods.UnixTimeStampToDateTime(Convert.ToDouble(temp[0])).ToString("yyyy-MM-dd hh:mm:ss");
                    record.duration = Int32.Parse(temp[1]);
                    record.clientAddress = temp[2];
                    record.squidReturnCode = temp[3].Split('/')[0];
                    record.httpReturnCode = temp[3].Split('/')[1];
                    record.bytes = Int32.Parse(temp[4]);
                    record.requestMethod = temp[5];
                    record.url = temp[6];
                    record.rfc931 = temp[7];
                    record.hierarchyCode = temp[8];
                    record.type = temp[9];

                    // http://www.bytechaser.com/en/articles/ckcwh8nsyt/display-progress-bar-in-console-application-in-c.aspx
                    dbCon.Insert(record);
                }

                line++;
            }
        }

    }
}
