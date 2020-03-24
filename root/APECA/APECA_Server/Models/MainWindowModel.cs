using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using APECA_Server.Scripts;
using APECA_Shared_Library;

namespace APECA_Server.Models
{
    public class MainWindowModel
    {
        private Server serverObj;

        public MainWindowModel()
        {
            serverObj = new Server();
        }

        public bool startServer()
        {
            return serverObj.Start();
        }
        public void stopServer()
        {
            serverObj.Stop();
        }
        public bool serverIsLive()
        {
            return serverObj.isLive;
        }

        public void setListenIP(string IP)
        {
            serverObj.localAddress = IPAddress.Parse(IP);
        }
        public string getListenIP()
        {
            if (serverObj.localAddress != null)
            {
                return serverObj.localAddress.ToString();
            }
            else
            {
                return "None";
            }
        }
        public ObservableCollection<Client> getClients()
        {
            return serverObj.clients; 
        }
    }
}
