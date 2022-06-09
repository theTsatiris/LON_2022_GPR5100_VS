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
            Dictionary<IPEndPoint, string> connectedClients = new Dictionary<IPEndPoint, string>();

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
                string[] splitText = text.Split('|');
                if(splitText[0] == "<342%$%^#$kjhjfGDhved%^jkgkF6745eo98%3f>")
                {
                    Console.WriteLine("NEW CONNECTION REQUEST!");
                    //adding new client
                    if (!connectedClients.ContainsKey((IPEndPoint)senderEndp))
                    {
                        if (splitText.Length > 1)
                        {
                            Console.WriteLine("ADDED CLIENT " + ((IPEndPoint)senderEndp).ToString());
                            connectedClients.Add((IPEndPoint)senderEndp, splitText[1]);
                        }
                        else
                            Console.WriteLine("ILLEGAL CONNECTION REQUEST!");
                    }
                    else
                    {
                        if (splitText.Length > 1)
                        {
                            Console.WriteLine("UPDATED CLIENT " + ((IPEndPoint)senderEndp).ToString());
                            connectedClients[(IPEndPoint)senderEndp] = splitText[1];
                        }
                        else
                        {
                            Console.WriteLine("UPDATED CLIENT " + ((IPEndPoint)senderEndp).ToString());
                            connectedClients[(IPEndPoint)senderEndp] = "ANONYMOUS";
                        }
                    }
                    byte[] acknowledgmentData = System.Text.Encoding.ASCII.GetBytes("<342%$%^#$kjhjfGDhved%^jkgkF6745eo98%3f>");
                    sock.SendTo(acknowledgmentData, (IPEndPoint)senderEndp);
                }
                else
                {
                    if(connectedClients.ContainsKey((IPEndPoint)senderEndp))
                    { 
                        //forwarding message to other clients
                        string senderIP = ((IPEndPoint)senderEndp).Address.ToString();
                        int senderPort = ((IPEndPoint)senderEndp).Port;

                        string newText = "[" + connectedClients[(IPEndPoint)senderEndp] + "] " + text; 
                        byte[] forwardingData = System.Text.Encoding.ASCII.GetBytes(newText);
                        foreach (IPEndPoint endp in connectedClients.Keys)
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
}
