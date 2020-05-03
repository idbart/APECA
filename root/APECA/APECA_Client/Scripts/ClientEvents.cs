using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using APECA_Shared_Library;

namespace APECA_Client.Scripts
{
    //Application wide static events
    public delegate void MessageHandler(BroadcastRequest message);
    public delegate void NotificationHandler(NotificationRequest notification);

    public static class ClientEvents
    {
        public static event MessageHandler messageReceived;
        public static void invokeMessageRecived(BroadcastRequest message)
        {
            messageReceived?.Invoke(message);
        }

        public static event MessageHandler messageSent;
        public static void invokeMessageSent(BroadcastRequest request)
        {
            messageSent?.Invoke(request);
        }

        public static event NotificationHandler notificationReceived;
        public static void invokeNotificationReceived(NotificationRequest notification)
        {
            notificationReceived(notification);
        }

        public static event Action settingsChanged;
        public static void invokeSettingsChanged()
        {
            settingsChanged?.Invoke();
        }
    }
}
