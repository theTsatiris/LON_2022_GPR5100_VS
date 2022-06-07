using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;

namespace ChatServer
{
    class ChatServer
    {
        static void Main(string[] args)
        {
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
                string text = System.Text.Encoding.ASCII.GetString(data, 0, numOfBytes);
                if(text == "<342%$%^#$kjhjfGDhved%^jkgkF6745eo98%3f>")
                {
                    Console.WriteLine("NEW CONNECTION REQUEST!");
                    //adding new client
                    if(!connectedClients.Contains((IPEndPoint)senderEndp))
                    {
                        Console.WriteLine("ADDED CLIENT " + ((IPEndPoint)senderEndp).ToString());
                        connectedClients.Add((IPEndPoint)senderEndp);
                    }

                    byte[] acknowledgmentData = System.Text.Encoding.ASCII.GetBytes("<342%$%^#$kjhjfGDhved%^jkgkF6745eo98%3f>");
                    sock.SendTo(acknowledgmentData, (IPEndPoint)senderEndp);
                }
                else
                {
                    //forwarding message to other clients
                    string senderIP = ((IPEndPoint)senderEndp).Address.ToString();
                    int senderPort = ((IPEndPoint)senderEndp).Port;
                    byte[] forwardingData = System.Text.Encoding.ASCII.GetBytes(text);
                    foreach (IPEndPoint endp in connectedClients)
                    {
                        if((endp.Address.ToString() != senderIP) || (endp.Port != senderPort))
                        {
                            sock.SendTo(forwardingData, endp);
                        }
                    }
                }
            }
        }
    }
}
