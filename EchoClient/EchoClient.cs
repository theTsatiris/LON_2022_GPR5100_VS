using System;
using System.Net;
using System.Net.Sockets;

namespace EchoClient
{
    class EchoClient
    {
        static void Main(string[] args)
        {
            //create remote endpoint
            //IPAddress remoteAddr = IPAddress.Parse("10.44.51.95");
            IPAddress remoteAddr = IPAddress.Broadcast; //IPAddress.Broadcast is equiavalent to IPAddress.Parse("255.255.255.255")

            IPEndPoint destinationEndPoint = new IPEndPoint(remoteAddr, 55555);

            Console.WriteLine("Sending text data to " + destinationEndPoint.ToString());

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            //YOU NEED THIS ONLY WHEN BROADCASTING
            sock.EnableBroadcast = true;

            while(true)
            {
                string text = Console.ReadLine();

                byte[] data = System.Text.Encoding.ASCII.GetBytes(text);

                sock.SendTo(data, destinationEndPoint);
            }
        }
    }
}
