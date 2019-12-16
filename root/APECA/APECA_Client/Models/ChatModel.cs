using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APECA_Shared_Library;
using APECA_Client.Scripts;

namespace APECA_Client.Models
{
    public class ChatModel
    {
        public List<BrodcastRequest> messagesToDisplay;
        public ChatModel()
        {

        }

        public void sendMessage(string message)
        {
            BrodcastRequest request = new BrodcastRequest() { message = message, userName = Properties.Settings.Default.Config.userName };
            ClientEvents.invokeMessageSent(request);
        }
    }
}
