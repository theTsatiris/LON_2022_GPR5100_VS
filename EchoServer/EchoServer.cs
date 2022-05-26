using System;
using System.Net;
using System.Net.Sockets;

namespace EchoServer
{
    class EchoServer
    {
        static void Main(string[] args)
        {
            //SIMPLE NSLOOKUP LOOKALIKE
            /*bool killProcess = false;
            while (!killProcess)
                killProcess = DnsLookup();*/

            IPAddress addr = IPAddress.Any;//IPAddress.Any is equivalent to IPAddress.Parse("0.0.0.0")

            IPEndPoint endPoint = new IPEndPoint(addr, 55555); //five fives :D

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            sock.Bind(endPoint);

            Console.WriteLine("Opening socket, echoing from " + addr.ToString() + ":" + endPoint.Port);

            byte[] data = new byte[1024];

            while(true)
            {
                int numOfBytes = sock.Receive(data);
                string text = System.Text.Encoding.ASCII.GetString(data, 0, numOfBytes);
                Console.WriteLine(text);
            }
        }

        //DNS service example
        static bool DnsLookup()
        {
            Console.WriteLine("Please enter a host name...");
            string hostname = Console.ReadLine();
            if (hostname == "exit")
                return true;
            Console.WriteLine(hostname + " resolves to:");
            IPAddress[] addresses = Dns.GetHostAddresses(hostname);
            foreach(IPAddress addr in addresses)
            {
                Console.WriteLine("- " + addr.ToString());
            }
            return false;
        }
    }
}
