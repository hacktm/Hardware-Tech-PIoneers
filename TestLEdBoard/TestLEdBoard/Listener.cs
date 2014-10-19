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
using System.Net;
using System.Net.Sockets;


namespace TestLEdBoard
{
    public class Listener
    {

        MainInterface messagefromclient;
        private EventWaitHandle LetOneTrough;

        public Thread ClientThread;


        public List<MainInterface> Clients = new List<MainInterface>();


        public Listener() 
        {
            LetOneTrough = new EventWaitHandle(false, EventResetMode.AutoReset);

            ClientThread = new Thread(ReceiveFunction);
            ClientThread.Start();
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
        //receiver
        public void Register_Receiver(MainInterface NewClient)
        {
            //add incoming client to the registry 
            Clients.Add(NewClient);
        }


        //-singleton
        void ReceiveFunction()
        {

            AssemblyName asmName = Assembly.GetExecutingAssembly().GetName();

            try
            {
                IPAddress[] ipList = Dns.GetHostEntry("").AddressList;
                IPAddress ipHost = null;
                IPEndPoint listeningEndPoint = null;
                TcpListener tcpListener = null;

                // find an IPv4 address.
                foreach (IPAddress ip in ipList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipHost = ip;
                        break;
                    }
                }
                
                listeningEndPoint = new IPEndPoint(ipHost, 8000);
                tcpListener = new TcpListener(listeningEndPoint);

                byte[] rgbMsg = new byte[1024];
                TcpClient tcpClient = null;
                NetworkStream tcpStream = null;

                tcpListener.Start();
                do
                {
                   // Console.WriteLine("Listening on port {0}", listeningPort.ToString());

                    // block until we get a client
                    tcpClient = tcpListener.AcceptTcpClient();
                    tcpStream = tcpClient.GetStream();
                    tcpStream.ReadTimeout = 5000;

                    // Console.WriteLine("Detected Client");

                    while (tcpClient.Connected)
                    {
                        int cbRead = 0;

                        try
                        {
                            while ((cbRead = tcpStream.Read(rgbMsg, 0, rgbMsg.Length)) > 0)
                            {
                                Encoding ascii = Encoding.ASCII;
                                string sz = ascii.GetString(rgbMsg, 0, cbRead);

                                // Console.WriteLine("Writing string:");
                               // Console.Write(sz);
                                //------------------------------------------------
                                
                                    messagefromclient = this.Clients.ElementAtOrDefault(0);
                                    messagefromclient.On_msg_recieved(sz);
                               

                               
                            }
                           /* else
                            {
                                break;
                            }
                            * */
                        }
                        catch
                        {
                            break;
                        }
                    }
                 //   tcpStream.Close();
                //    tcpClient.Close();

                } while (true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
