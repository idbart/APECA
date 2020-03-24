using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using APECA_Shared_Library;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace APECA_Server.Scripts
{
    public class Server
    {
        public ObservableCollection<Client> clients;
        public bool isLive { get; private set; }

        public IPAddress localAddress;
        private readonly int listenPort = 6955;

        private TcpListener tcpServer; 

        public Server()
        {
            clients = new ObservableCollection<Client>();
            BindingOperations.EnableCollectionSynchronization(clients, this);

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
            serverThread.IsBackground = true;

            serverThread.Start();

            return true;
        }
        public void Stop()
        {
            foreach(Client client in clients)
            {
                client.tcpClient.Close();
            }

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
                    clientThread.IsBackground = true;

                    clientThread.Start();
                }

                Thread.Sleep(100);
            }
            tcpServer.Stop();
        }
        private void handleClient(TcpClient client)
        {
            Client user = null;

            while(client.Connected)
            {
                NetworkStream stream = client.GetStream();
                if (stream.DataAvailable)
                {
                    byte[] buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);

                    if (SharedPacketTranslation.isConnectionRequest(buffer))
                    {
                        IPAddress clientIP = IPAddress.Parse(client.Client.RemoteEndPoint.ToString().Split(':')[0]);
                        string username = SharedEncoding.decodeConnectionRequest(buffer).userName;

                        Client thisClient = null;
                        foreach(Client clide in clients)
                        {
                            if (clide.userName == username && clide.publicIP.ToString() == clientIP.ToString())
                            {
                                thisClient = clide;
                                break;
                            }
                        }

                        if (thisClient != null)
                        {
                            user = thisClient;

                            thisClient.tcpClient = client;
                            thisClient.isConnected = true;
                        }
                        else
                        {
                            Client newClient = new Client() { userName = username, isConnected = true, publicIP = clientIP, tcpClient = client };
                            user = newClient;

                            clients.Add(newClient);
                        }
                    }
                    else if (SharedPacketTranslation.isDisconnectRequest(buffer))
                    {
                        user.isConnected = false;
                    }
                    else if (SharedPacketTranslation.isBrodcastRequest(buffer))
                    {
                        sendMessageToAllConnectedClients(buffer);
                    }
                }

                Thread.Sleep(100);
            }

            user.isConnected = false;
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
