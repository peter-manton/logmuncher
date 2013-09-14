using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SquidLogAnalyzer
{
    class LogRecord : LogFactory
    {
        public string dateStamp;

        public int duration;

        public string clientAddress;

        public string squidReturnCode;

        public string httpReturnCode;

        public int bytes;

        public string requestMethod;

        public string url;

        public string rfc931;

        public string hierarchyCode;

        public string type;

    }
}
