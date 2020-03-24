using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APECA_Client.Scripts;
using APECA_Shared_Library;
using System.Collections.ObjectModel;

namespace APECA_Client.Models
{
    public class MainWindowModel
    {
        public ClientConnection connection;
        public List<Client> clients;

        public MainWindowModel()
        {
            connection = new ClientConnection();
            clients = new List<Client>();
        }

        public bool connectToServer()
        {
            return connection.tryConnectToServer();
        }
        public void dissconnectFromServer()
        {
            connection.disconnect();
        }
        public SettingsConfig getConfig()
        {
            return connection.config;
        }
    }
}
