using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ChatClient
{
    class ChatClient
    {
        public static ChatClient client = new ChatClient();

        bool connectionEstablished;

        static void ReceiverProc(Object obj)
        {
            byte[] data = new byte[1024];
            Socket sock = (Socket)obj;

            //CHECKING IF ITS THE SAME IP IS NOT RELEVANT ANYMORE
            /*string localIP = "";
            try
            {
                localIP = GetLocalIPAddress();
            }
            catch
            {
                //ALTERNATIVE COURSE OF ACTION, IF CODE IN "TRY" FAILS
            }*/

            EndPoint senderEndpoint = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                int numOfBytes = sock.ReceiveFrom(data, ref senderEndpoint);
                string text = System.Text.Encoding.ASCII.GetString(data, 0, numOfBytes);
                if(text == "<342%$%^#$kjhjfGDhved%^jkgkF6745eo98%3f>")
                {
                    client.connectionEstablished = true;
                    Console.WriteLine("Connection established!");
                }
                else
                {
                    Console.WriteLine(text);
                }
            }
        }

        static string GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress addr in host.AddressList)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    return addr.ToString();
                }
            }
            throw new Exception("ERROR: No network adapters with an IPv4 address in the system!");
        }

        static void Main(string[] args)
        {
            client.connectionEstablished = false;

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

            Thread receiverThread = new Thread(new ParameterizedThreadStart(ReceiverProc));
            receiverThread.Start(sock);

            Console.WriteLine("Please type a username:");
            string username = Console.ReadLine();

            //Send request to enter the chat room to server
            byte[] connectionData = System.Text.Encoding.ASCII.GetBytes("<342%$%^#$kjhjfGDhved%^jkgkF6745eo98%3f>|" + username);
            sock.SendTo(connectionData, destinationEndPoint);
            Console.WriteLine("Sent connection request...");

            while (true)
            {
                Console.WriteLine(".");
                if (client.connectionEstablished)
                { 
                    string text = Console.ReadLine();
                    byte[] outboundData = System.Text.Encoding.ASCII.GetBytes(text);
                    sock.SendTo(outboundData, destinationEndPoint);
                }
            }
        }
    }
}
