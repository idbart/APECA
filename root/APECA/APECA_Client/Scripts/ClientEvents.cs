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
    public delegate void MessageHandler(BrodcastRequest message);

    public static class ClientEvents
    {
        public static event MessageHandler messageRecived;
        public static void invokeMessageRecived(BrodcastRequest message)
        {
            messageRecived(message);
        }

        public static event MessageHandler messageSent;
        public static void invokeMessageSent(BrodcastRequest request)
        {
            messageSent(request);
        }

        public static event Action settingsChanged;
        public static void invokeSettingsChanged()
        {
            settingsChanged();
        }
    }
}
