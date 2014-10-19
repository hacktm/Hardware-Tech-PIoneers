using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Win32.SafeHandles;
using System.Reflection;
using System.Threading;
using System.Timers;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestLEdBoard
{
    public class Listener
    {

        private EventWaitHandle LetOneTrough;

        public Thread ClientThread;

        public Listener() 
        {
            LetOneTrough = new EventWaitHandle(false, EventResetMode.AutoReset);

            ClientThread = new Thread(ReceiveFunction);
        }

        //singleton 
        private static Listener _instance;
        public static Listener Instance()
        {
            // Note: this is not thread safe.(to be improoved)
            if (_instance == null)
            {
                _instance = new Listener();
            }
            return _instance;
        }
        //-singleton
        void ReceiveFunction()
        {

            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port =5001;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];

                String data = null;

                // Enter the listening loop. 
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client. 
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

    }
}
