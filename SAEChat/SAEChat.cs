using System;
using System.Net;
using System.Net.Sockets;

namespace SAEChat
{
    class SAEChat
    {
        static void Main(string[] args)
        {

            //OUTBOUND TRAFFIC INFRASTRUCTURE
            IPAddress remoteAddr = IPAddress.Broadcast; //IPAddress.Broadcast is equiavalent to IPAddress.Parse("255.255.255.255")
            IPEndPoint destinationEndPoint = new IPEndPoint(remoteAddr, 55555);

            Console.WriteLine("Sending text data to " + destinationEndPoint.ToString());

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.EnableBroadcast = true;
            //*******************************

            //INBOUND TRAFFIC INFRASTRUCTURE
            IPAddress addr = IPAddress.Any;//IPAddress.Any is equivalent to IPAddress.Parse("0.0.0.0")
            IPEndPoint endPoint = new IPEndPoint(addr, 55555); //five fives :D

            sock.Bind(endPoint);
            Console.WriteLine("Opening socket, echoing from " + addr.ToString() + ":" + endPoint.Port);
            //*******************************

            byte[] data = new byte[1024];
            while (true)
            {
                string text = Console.ReadLine();
                data = System.Text.Encoding.ASCII.GetBytes(text);
                sock.SendTo(data, destinationEndPoint);

                int numOfBytes = sock.Receive(data);
                text = System.Text.Encoding.ASCII.GetString(data, 0, numOfBytes);
                Console.WriteLine(text);
            }
        }
    }
}
