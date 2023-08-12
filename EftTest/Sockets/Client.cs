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
        private const int BufferSize = 1000000;
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
            byte[] receivedbytes = new byte[BufferSize];
            byte[] receiveddata = new byte[0];

            while (true)
            {
                int received = Sock.Receive(receivedbytes, 0, receivedbytes.Length, SocketFlags.None);

                if (received <= 0)
                    break;

                Array.Resize(ref receiveddata, receiveddata.Length + received);
                Array.Copy(receivedbytes, 0, receiveddata, receiveddata.Length - received, received);

                if (received < receivedbytes.Length)
                    break;
            }

            string str = Encoding.UTF8.GetString(receiveddata);
            return str;
        }
    }
}
