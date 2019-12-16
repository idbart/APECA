using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APECA_Client.Scripts;
using APECA_Shared_Library;

namespace APECA_Client.Models
{
    public class MainWindowModel
    {
        public ClientConnection connection = new ClientConnection();
        public List<BrodcastRequest> messages = new List<BrodcastRequest>();
        public List<Client> clients = new List<Client>();

        public MainWindowModel()
        {
            ClientEvents.messageRecived += handelIncomingMessage;
        }
        public void handelIncomingMessage(BrodcastRequest message)
        {
            messages.Add(message);
        }

        public bool connectToServer()
        {
            return connection.tryConnectToServer();
        }
        public SettingsConfig getConfig()
        {
            return connection.config;
        }
    }
}
