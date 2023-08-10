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
            // Start the server
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Listening = socket;

            IPAddress ipAddress = IPAddress.Any;
            IPEndPoint endpoint = new IPEndPoint(ipAddress, 51000);

            Listening.Bind(endpoint); // Bind the connection rules to the listening socket
            Listening.Listen(10);     // Keep socket open with maximum backlog of 10 connections

        }
        public static void AcceptClients()
        {
            while (true)
            {
                Socket clientsocket = Listening.Accept(); // Accept a new client connection


                TCPClient = new Sockets.Client(clientsocket);
                return;
            }
        }


    }
}
