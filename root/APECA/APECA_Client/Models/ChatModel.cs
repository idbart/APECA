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
        public ObservableCollection<BroadcastRequest> messagesToDisplay;

        public ChatModel()
        {
            messagesToDisplay = new ObservableCollection<BroadcastRequest>();
            BindingOperations.EnableCollectionSynchronization(messagesToDisplay, this);

            ClientEvents.messageReceived += this.receiveMessage;
            ClientEvents.notificationReceived += this.receiveNotification;
        }

        public void receiveMessage(BroadcastRequest message)
        {
            messagesToDisplay.Add(message);
        }
        public void receiveNotification(NotificationRequest notification)
        {
            messagesToDisplay.Add(new BroadcastRequest() { userName = notification.message });
        }
        public void sendMessage(string message)
        {
            BroadcastRequest request = new BroadcastRequest() { message = message, userName = Properties.Settings.Default.Config.userName };
            ClientEvents.invokeMessageSent(request);
        }
    }
}
