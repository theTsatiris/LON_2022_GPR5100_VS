using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace P2P_ChatTracker
{
    class P2P_ChatTracker
    {
        static void Main(string[] args)
        {
            //Dictionary<IPEndPoint, string> connectedClients = new Dictionary<IPEndPoint, string>();
            List<IPEndPoint> connectedClients = new List<IPEndPoint>();

            IPAddress addr = IPAddress.Any;//IPAddress.Any is equivalent to IPAddress.Parse("0.0.0.0")
            IPEndPoint endPoint = new IPEndPoint(addr, 55555); //five fives :D

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.Bind(endPoint);

            Console.WriteLine("Opening socket, echoing from " + addr.ToString() + ":" + endPoint.Port);

            EndPoint senderEndp = new IPEndPoint(IPAddress.Broadcast, 0);

            byte[] data = new byte[1024];
            while (true)
            {
                int numOfBytes = sock.ReceiveFrom(data, ref senderEndp);
                Console.WriteLine("RECEIVED DATA!!!");
                IPEndPoint newClient = (IPEndPoint)senderEndp;
                string text = System.Text.Encoding.ASCII.GetString(data, 0, numOfBytes);
                if (text == "<342%$%^#$kjhjfGDhved%^jkgkF6745eo98%3f>")
                {
                    Console.WriteLine("NEW CONNECTION REQUEST!");
                    //adding new client
                    if (!connectedClients.Contains(newClient))
                    {
                        Console.WriteLine("ADDED CLIENT " + newClient.ToString());
                        
                        foreach(IPEndPoint endp in connectedClients)
                        {
                            //Send current client data to new client
                            byte[] dataToNewClient = System.Text.Encoding.ASCII.GetBytes("<ENDPOINTINFO>|" + endp.ToString());
                            sock.SendTo(dataToNewClient, newClient);

                            //Send new client data to current client
                            byte[] dataToCurrClient = System.Text.Encoding.ASCII.GetBytes("<ENDPOINTINFO>|" + newClient.ToString());
                            sock.SendTo(dataToCurrClient, endp);
                        }
                        connectedClients.Add(newClient);
                    }
                }
            }
        }
    }
}
