using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Windows;
using System.Net.Sockets;
using APECA_Shared_Library;
using System.IO;
using System.Text.Json;

namespace APECA_Client.Scripts
{
    //All the connection and packet handeling logic
    public class ClientConnection
    {
        private TcpClient client;
        private NetworkStream stream;
        public SettingsConfig config;
        

        public ClientConnection()
        {    
            ClientEvents.settingsChanged += () => {
                tryConnectToServer();
            };
            tryConnectToServer();  
        }

        public bool tryConnectToServer()
        {
            if(client != null)
            {
                disconnect();
            }

            config = Properties.Settings.Default.Config;

            return connect();
        }
        private bool connect()
        {
            if(config == null || config.serverIP == default(string) || config.userName == default(string))
            {
                MessageBox.Show("Cannot connect to server without proper configuration");

                return false;
            }
            else
            {
                try
                {
                    client = new TcpClient(config.serverIP, 6955);
                    stream = client.GetStream();

                    ConnectionRequest connReq = new ConnectionRequest { userName = config.userName };
                    byte[] connReqPacket = SharedEncoding.encodeConnectionRequest(connReq);

                    stream.Write(connReqPacket, 0, connReqPacket.Length);

                    ClientEvents.messageSent += sendMessage;

                    Thread connThread = new Thread(Main);
                    connThread.IsBackground = true;

                    connThread.Start();

                    return true;
                }
                catch(Exception exe)
                {
                    MessageBox.Show(exe.ToString());

                    return false;
                }
            }
        }
        public void disconnect()
        {
            if(stream != null && client != null)
            {
                DisconnectionRequest request = new DisconnectionRequest() { userName = config.userName };
                byte[] packet = SharedEncoding.encodeDisconnectionRequest(request);

                try
                {
                    stream.Write(packet, 0, packet.Length);
                }
                finally
                {
                    ClientEvents.messageSent -= sendMessage;
                    client.Close();
                }
            }
        }
        public void sendMessage(BroadcastRequest request)
        {
            byte[] packet = SharedEncoding.encryptBroadcastRequest(request, config.key);

            if(stream != null)
            {
                try
                {
                    stream.WriteAsync(packet, 0, packet.Length);
                }
                catch(Exception e)
                {
                    MessageBox.Show("ERROR: Cannot send message");
                }
            }
            else
            {
                MessageBox.Show("ERROR: Cannot send message");
            }
        }

        private void Main()
        {
            while(client.Connected)
            {
                if(stream.DataAvailable)
                {
                    byte[] buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);

                    if(SharedPacketTranslation.isBrodcastRequest(buffer))
                    {
                        BroadcastRequest request = SharedEncoding.decryptBroadcastRequest(buffer, config.key);
                        ClientEvents.invokeMessageRecived(request);
                    }
                    else if (SharedPacketTranslation.isNotificationRequest(buffer))
                    {
                        NotificationRequest notification = SharedEncoding.decodeNotificationRequest(buffer);
                        ClientEvents.invokeNotificationReceived(notification);
                    }
                }

                Thread.Sleep(100);
            }
        }
    }
}
