using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace P2P_ChatClient
{
    public class ThreadParams
    {
        public Socket socket;
        public List<IPEndPoint> clients;
    }

    class P2P_ChatClient
    {
        static void ReceiverProc(Object obj)
        {
            byte[] data = new byte[1024];

            ThreadParams parameters = (ThreadParams)obj;

            while (true)
            {
                int numOfBytes = parameters.socket.Receive(data);
                string text = System.Text.Encoding.ASCII.GetString(data, 0, numOfBytes);
                string[] splitText = text.Split('|');
                if(splitText[0] == "<ENDPOINTINFO>")
                {
                    string[] endpointData = splitText[1].Split(':');
                    IPAddress endpointAddr = IPAddress.Parse(endpointData[0]);
                    int endpoinPort = int.Parse(endpointData[1]);
                    IPEndPoint endp = new IPEndPoint(endpointAddr, endpoinPort);
                    parameters.clients.Add(endp);
                }
                else
                {
                    Console.WriteLine(text);
                }
            }
        }

        static void Main(string[] args)
        {
            List<IPEndPoint> connectedClients = new List<IPEndPoint>();

            //OUTBOUND TRAFFIC INFRASTRUCTURE
            Console.WriteLine("Please type the IP address of the server and press enter:");
            string serverIP = Console.ReadLine();
            IPAddress destinationAddr = IPAddress.Parse(serverIP);
            IPEndPoint destinationEndPoint = new IPEndPoint(destinationAddr, 55555); //five fives :D

            Console.WriteLine("Sending text data to " + destinationEndPoint.ToString());

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //INBOUND TRAFFIC INFRASTRUCTURE
            IPAddress localAddr = IPAddress.Any;//IPAddress.Any is equivalent to IPAddress.Parse("0.0.0.0")

            Console.WriteLine("Please type the port number you want your client to operate on:");
            int localPort = int.Parse(Console.ReadLine());
            IPEndPoint localEndPoint = new IPEndPoint(localAddr, localPort);

            sock.Bind(localEndPoint);
            Console.WriteLine("Opening socket for incomming messages...");

            ThreadParams parameters = new ThreadParams();
            parameters.socket = sock;
            parameters.clients = connectedClients;

            Thread receiverThread = new Thread(new ParameterizedThreadStart(ReceiverProc));
            receiverThread.Start(parameters);

            Console.WriteLine("Please type a username:");
            string username = Console.ReadLine();

            //Send request to enter the chat room to server
            byte[] connectionData = System.Text.Encoding.ASCII.GetBytes("<342%$%^#$kjhjfGDhved%^jkgkF6745eo98%3f>");
            sock.SendTo(connectionData, destinationEndPoint);
            Console.WriteLine("Sent connection request...");

            while(true)
            {
                string text = Console.ReadLine();
                text = "[" + username + "] " + text;
                byte[] outboundData = System.Text.Encoding.ASCII.GetBytes(text);
                foreach(IPEndPoint destination in connectedClients)
                { 
                    sock.SendTo(outboundData, destination);
                }
            }
        }
    }
}
