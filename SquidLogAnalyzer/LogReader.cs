using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SquidLogAnalyzer
{
    class LogReader : LogFactory
    {
        public LogReader(string logFile)
        {
            this.logFile = logFile;
        }

        public string logFile;
        private List<string> buffer = new List<string>();

        public bool Read()
        {
            try
            {
                using (StreamReader reader = new StreamReader(logFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        buffer.Add(line); // Add to list.

                        if (Options.debug == true)
                        {
                            Console.WriteLine(line); // Write to console.
                        }
                        
                    }
                }

                return true;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("ERROR: The input file you specified was not found.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
                return false;
            }
        }

        public List<string> Output
        {
            get
            {
                return this.buffer;
            }
            set
            {
                this.buffer = null;
            }
        }
    }
}
