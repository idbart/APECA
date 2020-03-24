using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APECA_Shared_Library;
using APECA_Client.Scripts;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace APECA_Client.Models
{
    public class ChatModel
    {
        public ObservableCollection<BrodcastRequest> messagesToDisplay;

        public ChatModel()
        {
            messagesToDisplay = new ObservableCollection<BrodcastRequest>();
            BindingOperations.EnableCollectionSynchronization(messagesToDisplay, this);

            ClientEvents.messageRecived += this.receiveMessage;
        }

        public void receiveMessage(BrodcastRequest message)
        {
            messagesToDisplay.Add(message);
        }
        public void sendMessage(string message)
        {
            BrodcastRequest request = new BrodcastRequest() { message = message, userName = Properties.Settings.Default.Config.userName };
            ClientEvents.invokeMessageSent(request);
        }
    }
}
