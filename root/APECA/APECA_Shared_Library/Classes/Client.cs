using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;

namespace APECA_Shared_Library
{
    public class Client : INotifyPropertyChanged
    {
        public string userName { get; set; }

        public IPAddress publicIP { get; set; }

        private bool _isConnected;
        public bool isConnected 
        { 
            get 
            { 
                return _isConnected; 
            } 
            set 
            {
                _isConnected = value;
                NotifyPropertyChanged("isConnected");
            } 
        }

        public TcpClient tcpClient;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
