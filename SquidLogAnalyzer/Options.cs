using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SquidLogAnalyzer
{
    static class Options
    {
        private static DateTime dt = DateTime.Now;
        public static bool daemonMode = false;
        public static int listenMode = 0;
        public static string configrationFile = String.Empty;
        //public static string generate = dt.ToString("yyyy-MM-dd hh:mm:ss");
        public static string generate = String.Empty;
        public static string slurp = String.Empty;
        public static bool debug = true;

        public static void ShowUsage()
        {
            Console.WriteLine("LogMuncher for Squid Proxy 1.0");
            Console.WriteLine();
            Console.WriteLine("Usage: \r\n \r\n-r --readlog  : Read directly from log file");
            Console.WriteLine("-l --listen   : Listen on UDP port for incoming logs from squid (default 9050)");
            Console.WriteLine("-d --deamon   : Daemon mode (run in background - only with --listen)");
            Console.WriteLine("-c --config   : Specify configuration file (required)");
            Console.WriteLine("-g --generate : Generate report for specified date (if none uses current date)");
            Console.WriteLine("-s --slurp    : Import logs from logfile (must be in standard squid format!)");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(1);

        }

    }
}
