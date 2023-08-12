using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace EftTest
{
    class SocketServer
    {
        private static Socket Listening;
        public static Sockets.Client TCPClient;

        public static void CreateServer()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Listening = socket;

            IPAddress ipaddress = IPAddress.Any;
            IPEndPoint endpoint = new IPEndPoint(ipaddress, 51000);

            Listening.Bind(endpoint); 
            Listening.Listen(10);

        }
        public static void AcceptClients()
        {
            while (true)
            {
                Socket clientsocket = Listening.Accept(); 


                TCPClient = new Sockets.Client(clientsocket);
                return;
            }
        }


    }
}
