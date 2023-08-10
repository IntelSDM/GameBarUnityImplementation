using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace EftTest.Sockets
{
    class Client
    {
        private const int BufferSize = 4096;
        public Socket Sock;
        public Client(Socket client)
        {
            Sock = client;
        }
        public void SendText(string text)
        {
            text += "|";
            byte[] plaintext = Encoding.UTF8.GetBytes(text);
            int result = Sock.Send(plaintext, 0, plaintext.Length, SocketFlags.None);
        }

        public string ReceiveText()
        {
            byte[] receivedBytes = new byte[BufferSize];
            byte[] receivedData = new byte[0];

            while (true)
            {
                int received = Sock.Receive(receivedBytes, 0, receivedBytes.Length, SocketFlags.None);

                if (received <= 0)
                    break;

                Array.Resize(ref receivedData, receivedData.Length + received);
                Array.Copy(receivedBytes, 0, receivedData, receivedData.Length - received, received);

                if (received < receivedBytes.Length)
                    break;
            }

            string str = Encoding.UTF8.GetString(receivedData);
            return str;
        }
    }
}
