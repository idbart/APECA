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
    //Model for saving user confurations
    public class SettingsConfig
    {
        public string userName { get; set; }
        public string serverIP { get; set; }
        public byte[] key { get; set; }

        public string getKey()
        {
            return SharedEncoding.decodeString(key);
        }
        public bool setKey(string input)
        {
            byte[] bytes = SharedEncoding.encodeString(input);
            if (bytes.Length == 16)
            {
                this.key = bytes;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    //Application wide static events
    public delegate void MessageHandler(BrodcastRequest message);
    public static class ClientEvents
    {
        public static event MessageHandler messageRecived;
        public static void invokeMessageRecived(BrodcastRequest message)
        {
            messageRecived.Invoke(message);
        }

        public static event MessageHandler messageSent;
        public static void invokeMessageSent(BrodcastRequest request)
        {
            messageSent.Invoke(request);
        }
    }

    //All the connection and packet handeling logic
    public class ClientConnection
    {
        private TcpClient client;
        private NetworkStream stream;
        public SettingsConfig config = Properties.Settings.Default.Config;
        

        public ClientConnection()
        {
            Properties.Settings.Default.SettingsLoaded += (sender, e) => { tryConnectToServer(); }; 
            Properties.Settings.Default.PropertyChanged += (sender, e) => { tryConnectToServer(); }; 
        }

        public bool tryConnectToServer()
        {
            if(client != null)
            {
                disconnect();
            }

            try
            {
                connect();
                return true;
            }
            catch(Exception exe)
            {
                MessageBox.Show(exe.ToString());
                return false;
            }
        }
        private void connect()
        {
            if(config == null || config.serverIP == default(string) || config.userName == default(string))
            {
                throw new Exception("Cannot connect to server without proper configuration");
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
                    connThread.Start();
                }
                catch(Exception exe)
                {
                    MessageBox.Show(exe.ToString());
                }
            }
        }
        private void disconnect()
        {
            DisconnectionRequest request = new DisconnectionRequest() { userName = config.userName };
            byte[] packet = SharedEncoding.encodeDisconnectionRequest(request);

            stream.Write(packet, 0, packet.Length);
        }
        public void sendMessage(BrodcastRequest request)
        {
            byte[] packet = SharedEncoding.encryptBrodcastRequest(request, config.key);

            stream.Write(packet, 0, packet.Length);
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
                        BrodcastRequest request = SharedEncoding.decryptBrodcastRequest(buffer, config.key);
                        ClientEvents.invokeMessageRecived(request);
                    }
                }
            }
        }
    }
}
