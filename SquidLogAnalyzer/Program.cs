using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace SquidLogAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {

            // Check configuration
            HelperMethods.checkConfiguration(args);

            Console.WriteLine("Squid Log File Analyzer v1.0");
            Console.WriteLine();


            if (Options.daemonMode == true)
            {
                Console.WriteLine("Daemon mode not implemented yet.");
                Console.WriteLine("Finished - press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            if (String.IsNullOrEmpty(Options.generate) == false)
            {
                // Create the object here to save processing time!
                MysqlFactory dbCon = new MysqlFactory();

                Console.WriteLine("Writing data...");

                // Generate top url list
                LogWriter lwUrls = new LogWriter(@"reports\squid.html", "", dbCon.SelectTopUrls(DateTime.Parse(Options.generate)));
                lwUrls.WriteTopUrls();

                // Generate highest traffic volume from users
                LogWriter lwBytes = new LogWriter(@"reports\squid.html", "", dbCon.SelectTopBytes(DateTime.Parse(Options.generate)));
                lwBytes.WriteTopBytes();

                Console.WriteLine("Finished - press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            if (String.IsNullOrEmpty(Options.slurp) == false)
            {
                LogReader lr = new LogReader(Options.slurp);
                lr.Read();
                var results = lr.Output;
                int line = 1;
                Console.WriteLine("Total number of records to read: " + results.Count());

                // Add logs to database
                HelperMethods.addLog(results.ToArray());

                Console.WriteLine("Finished - press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            if (Options.listenMode != 0)
            {
                // Create UDP listner for squid messages over UDP
                LogListener loglis = new LogListener();
                Thread thr = new Thread(new ThreadStart(loglis.Listen));
                thr.Start();

                Console.WriteLine();
                Console.WriteLine("To terminate use Ctrl + C ...");
                Console.ReadKey();
            }


        }

    }
}
