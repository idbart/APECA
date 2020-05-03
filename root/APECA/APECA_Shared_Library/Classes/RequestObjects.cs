using System;
using System.Collections.Generic;
using System.Text;

namespace APECA_Shared_Library
{
    public class ConnectionRequest : IBroadcastable
    {
        public string userName { get; set; }
    }
    public class DisconnectionRequest : IBroadcastable
    {
        public string userName { get; set; }
    }
    public class BroadcastRequest : IBroadcastable
    {
        public string userName { get; set; }
        public string message { get; set; }
        public DateTime timestamp { get; private set; }
        public BroadcastRequest()
        {
            timestamp = DateTime.Now;
        }
    }
    public class NotificationRequest : IBroadcastable
    {
        public string message { get; set; }
        public DateTime timestamp { get; set; }

        public NotificationRequest()
        {
            timestamp = DateTime.Now;
        }
    }
}
