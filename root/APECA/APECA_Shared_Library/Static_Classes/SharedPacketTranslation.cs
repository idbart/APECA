using System;
using System.Collections.Generic;
using System.Text;

namespace APECA_Shared_Library
{
    public static class SharedPacketTranslation
    {
        public static bool isConnectionRequest(byte[] packet)
        {
            if (packet[0] == RequestCodes.connect)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isDisconnectRequest(byte[] packet)
        {
            if (packet[0] == RequestCodes.disconnect)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isBrodcastRequest(byte[] packet)
        {
            if (packet[0] == RequestCodes.broadcastMessage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isNotificationRequest(byte[] packet)
        {
            if (packet[0] == RequestCodes.notification)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
