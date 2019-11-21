using System;
using System.Collections.Generic;
using System.Text;

namespace APECA_Shared_Library
{
    public class ConnectionRequest
    {
        public string userName { get; set; }
    }
    public class DisconnectionRequest
    {
        public string userName { get; set; }
    }
    public class BrodcastRequest
    {
        public string userName { get; set; }
        public string message { get; set; }
    }
}
