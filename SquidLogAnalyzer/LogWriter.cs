using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SquidLogAnalyzer
{
    class LogWriter : LogFactory
    {
        public string logDestination;
        public string logFormat;
        public List<string>[] logInput;
        private StreamWriter writer;

        public LogWriter(string logDestination, string logFormat, List<string>[] logInput)
        {
            this.logDestination = logDestination;
            this.logFormat = logFormat;
            this.logInput = logInput;

            // Make sure reports directory exists!
            if (!Directory.Exists("reports"))
            {
                Directory.CreateDirectory("reports");
            }
        }

        public bool WriteTopUrls()
        {
            try
            {
                // Create new streamwriter object - create and open file for writing
                writer = new StreamWriter(logDestination.Replace(".html", "_" + Options.generate.Replace('/', '-') + "_TopUrls.html"));

                // Get template
                var allLines = File.ReadAllLines(@"templates\topurls_template.html").ToList();
                
                // Add data to table
                // Example Input: allLines.Insert(21, @"<tr><td>1</td><td>http://www.google.com</td><td>200</td></tr>");
                int lineNo = 21; // Line number at which the rows should be inserted 
                int position = 1;

                for (int i = 0; i < logInput[0].Count; i++)
                {
                    allLines.Insert(lineNo, @"<tr><td>" + position + @"</td><td>" + logInput[7].ToArray()[i] + @"</td><td>" + logInput[10].ToArray()[i] + @"</td></tr>");

                    lineNo++;
                    position++;
                }

                //Console.WriteLine("Debug:" + read);
                foreach (string str in allLines.ToArray())
                {
                    writer.WriteLine(str);
                }

                writer.Close();

            }
            catch (AccessViolationException)
            {
                Console.WriteLine("ERROR: Unable to write - the output file is already in use.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        public bool WriteTopBytes()
        {
            try
            {
                // Create new streamwriter object - create and open file for writing
                writer = new StreamWriter(logDestination.Replace(".html", "_" + Options.generate.Replace('/', '-') + "_TopBytes.html"));

                // Get template
                var allLines = File.ReadAllLines(@"templates\trafficsize_template.html").ToList();

                // Add data to table
                //allLines.Insert(21, @"<tr><td>1</td><td>http://www.google.com</td><td>200</td></tr>");
                int lineNo = 21; // Line number at which the rows should be inserted 
                int position = 1;

                for (int i = 0; i < logInput[0].Count; i++)
                {
                    allLines.Insert(lineNo, @"<tr><td>" + position + @"</td><td>" + logInput[5].ToArray()[i] + @"</td><td>" + logInput[7].ToArray()[i] + @"</td></tr>");

                    lineNo++;
                    position++;
                }

                //Console.WriteLine("Debug:" + read);
                foreach (string str in allLines.ToArray())
                {
                    writer.WriteLine(str);
                }

                writer.Close();

            }
            catch (AccessViolationException)
            {
                Console.WriteLine("ERROR: Unable to write - the output file is already in use.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }
    }
}
