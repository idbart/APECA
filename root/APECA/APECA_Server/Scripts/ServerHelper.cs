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
        public ObservableCollection<string> log;

        public bool isLive { get; private set; }

        public IPAddress localAddress;
        private readonly int listenPort = 6955;

        private TcpListener tcpServer; 

        public Server()
        {
            clients = new ObservableCollection<Client>();
            log = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(clients, this);
            BindingOperations.EnableCollectionSynchronization(log, this);

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
            log.Add($"server started at: {DateTime.Now.ToString()}");

            return true;
        }
        public void Stop()
        {
            foreach(Client client in clients)
            {
                client.tcpClient.Close();
            }

            isLive = false;
            log.Add($"server stopped at: {DateTime.Now.ToString()}");
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

                            log.Add($"{user.userName} reconnected at: {DateTime.Now.ToString()}");
                        }
                        else
                        {
                            Client newClient = new Client() { userName = username, isConnected = true, publicIP = clientIP, tcpClient = client };
                            user = newClient;

                            clients.Add(newClient);

                            log.Add($"{user.userName} connected at: {DateTime.Now.ToString()}");
                        }
                        sendMessageToAllConnectedClients(SharedEncoding.encodeNotificationRequest(new NotificationRequest() { message = $"{user.userName} has connected" }));
                    }
                    else if (SharedPacketTranslation.isDisconnectRequest(buffer))
                    {
                        user.isConnected = false;
                        log.Add($"{user.userName} disconnected at: {DateTime.Now.ToString()}");
                        sendMessageToAllConnectedClients(SharedEncoding.encodeNotificationRequest(new NotificationRequest() { message = $"{user.userName} has disconnected" }));
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
