using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SquidLogAnalyzer
{
    class LogListener
    {
        public void Listen()
        {

            int port = 9050;
            if (Options.listenMode != 0)
            {
                port = Options.listenMode;
            }

            int recv;
            byte[] data = new byte[4096];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);

            Socket newsock = new Socket(AddressFamily.InterNetwork,
                            SocketType.Dgram, ProtocolType.Udp);

            newsock.Bind(ipep);
            Console.WriteLine("Listening on UDP port " + port + " for squid log messages.");
            //Console.WriteLine("Waiting for a client...");

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);

            recv = newsock.ReceiveFrom(data, ref Remote);

            if (Options.debug == true)
            {
                Console.WriteLine("Message received from {0}:", Remote.ToString());
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
            }

            var initSeparatedMessages = Encoding.ASCII.GetString(data, 0, recv).Split((new char[] { '\n', '\r' }), StringSplitOptions.RemoveEmptyEntries);
            HelperMethods.addLog(initSeparatedMessages);

            while (true)
            {
                data = new byte[4096];
                recv = newsock.ReceiveFrom(data, ref Remote);

                if (Options.debug == true)
                {
                    Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
                }

                var separatedMessages = Encoding.ASCII.GetString(data, 0, recv).Split((new char[] {'\n', '\r'}), StringSplitOptions.RemoveEmptyEntries);
                HelperMethods.addLog(separatedMessages);
                newsock.SendTo(data, recv, SocketFlags.None, Remote);
            }
        }
    }
}
