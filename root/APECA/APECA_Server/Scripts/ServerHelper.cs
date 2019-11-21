using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using APECA_Shared_Library;

namespace APECA_Server.Scripts
{
    public class Server
    {
        public List<Client> clients = new List<Client>();
        public bool isLive { get; private set; }

        public IPAddress localAddress;
        private int listenPort = 6955;

        private TcpListener tcpServer; 

        public Server()
        {
            isLive = false;
        }

        public bool Start()
        {
            try
            {
                tcpServer = new TcpListener(localAddress, listenPort);
                tcpServer.Start();
            }
            catch(Exception exe)
            {
                MessageBox.Show(exe.ToString());
                return false;
            }

            isLive = true;

            Thread serverThread = new Thread(Main);
            serverThread.Start();

            return true;
        }
        public void Stop()
        {
            isLive = false;
        }

        private void Main()
        {
            while(isLive)
            {
                if (tcpServer.Pending())
                {
                    TcpClient client = tcpServer.AcceptTcpClient();

                    Thread clientThread = new Thread(() => handleClient(client));
                    clientThread.Start();
                }
            }
            tcpServer.Stop();
        }
        private void handleClient(TcpClient client)
        {
            while(client.Connected)
            {
                NetworkStream stream = client.GetStream();
                if (stream.DataAvailable)
                {
                    byte[] buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);

                    if (SharedPacketTranslation.isConnectionRequest(buffer))
                    {
                        IPAddress clientIP = IPAddress.Parse(client.Client.RemoteEndPoint.ToString());
                        string username = SharedEncoding.decodeConnectionRequest(buffer).userName;

                        Client thisClient = (Client)from c in clients where c.userName == username && c.publicIP.ToString() == clientIP.ToString() select c;
                        if (thisClient != null)
                        {
                            thisClient.tcpClient = client;
                            thisClient.isConnected = true;
                        }
                        else
                        {
                            clients.Add(new Client() { userName = username, isConnected = true, publicIP = clientIP, tcpClient = client });
                        }
                    }
                    else if (SharedPacketTranslation.isDisconnectRequest(buffer))
                    {
                        Client thisClient = (Client)from c in clients where c.tcpClient == client select c;

                        thisClient.isConnected = false;
                        thisClient.tcpClient.Close();
                    }
                    else if (SharedPacketTranslation.isBrodcastRequest(buffer))
                    {
                        sendMessageToAllConnectedClients(buffer);
                    }
                }
            }
        }

        private void sendMessageToAllConnectedClients(byte[] packet)
        {
            foreach(Client i in clients)
            {
                if(i.isConnected)
                {
                    NetworkStream stream = i.tcpClient.GetStream();

                    stream.Write(packet, 0, packet.Length);
                }
            }
        }
    }
}
