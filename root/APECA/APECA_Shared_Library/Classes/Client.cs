using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace APECA_Shared_Library
{
    public class Client
    {
        public string userName { get; set; }
        public IPAddress publicIP { get; set; }
        public bool isConnected { get; set; }
        public TcpClient tcpClient;
    }
}
