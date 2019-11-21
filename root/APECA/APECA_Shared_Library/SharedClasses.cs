using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace APECA_Shared_Library
{
    public static class SharedEncoding
    {
        public static string decodeString(byte[] buffer)
        {
            string returnValue = default(string);

            foreach (byte b in buffer)
            {
                if (b == 00)
                {
                    break;
                }
                else
                {
                    returnValue += Convert.ToChar(b).ToString();
                }
            }
            return returnValue;
        }
        public static string decodeEncryptedString(byte[] buffer, byte[] key)
        {
            byte[] trimmedPacket = SharedPacketManipulation.trimPacketFat(buffer);
            byte[] decryptedPacket = Cipher.decrypt(trimmedPacket, key);

            return decodeString(decryptedPacket);
        }

        public static byte[] encodeString(string message)
        {
            byte[] encodedPacket = new byte[Encoding.ASCII.GetByteCount(message)];
            encodedPacket = Encoding.ASCII.GetBytes(message);

            return encodedPacket;
        }
        public static byte[] encryptString(string message, byte[] key)
        {
            byte[] encodedMessage = encodeString(message);
            return Cipher.encrypt(encodedMessage, key);
        }
    }
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
            if (packet[0] == RequestCodes.brodcastMessage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public static class SharedPacketManipulation
    {
        public static byte[] trimPacketFat(byte[] buffer)
        {
            for (int i = buffer.Length - 1; i > 0; i--)
            {
                if (buffer[i] == 00)
                {
                    Array.Resize(ref buffer, i);
                }
                else
                {
                    break;
                }
            }

            return buffer;
        }
        public static byte[] trimRequestCode(byte[] buffer)
        {
            byte[] returnValue = new byte[buffer.Length - 1];
            Array.Copy(buffer, 1, returnValue, 0, returnValue.Length);

            return returnValue;
        }
    }

    public class Client
    {
        public string userName { get; set; }
        public IPAddress publicIP { get; set; }
        public bool isConnected { get; set; }
        public TcpClient tcpClient;
    }

    public static class RequestCodes
    {
        public static byte connect = 1;
        public static byte disconnect = 2;
        public static byte brodcastMessage = 3;
    }
}