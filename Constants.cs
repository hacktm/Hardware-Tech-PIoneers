using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestLEdBoard
{
    public static class Constants
    {
        private static  string theIP = "127.0.0.1";
        public static IPAddress IP = IPAddress.Parse(theIP);
       public static Int32 port = 5001;
    }
}
