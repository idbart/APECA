using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        private static T decodeUnencryptedRequest<T>(byte[] buffer)
        {
            byte[] data = SharedPacketManipulation.trimRequestCode(buffer);
            string jsonString = decodeString(data);

            return JsonSerializer.Deserialize<T>(jsonString);
        }
        private static byte[] encodeUnencryptedRequest(object request, byte code)
        {
            string jsonString = JsonSerializer.Serialize(request);
            byte[] encodedObject = encodeString(jsonString);

            byte[] returnValue = new byte[encodedObject.Length + 1];
            returnValue[0] = code;
            Array.Copy(encodedObject, 0, returnValue, 1, encodedObject.Length);

            return returnValue;
        }

        public static ConnectionRequest decodeConnectionRequest(byte[] buffer)
        {
            return decodeUnencryptedRequest<ConnectionRequest>(buffer);
        }
        public static byte[] encodeConnectionRequest(ConnectionRequest request)
        {
            return encodeUnencryptedRequest(request, RequestCodes.connect);
        }

        public static DisconnectionRequest decodeDisconnectionRequest(byte[] buffer)
        {
            return decodeUnencryptedRequest<DisconnectionRequest>(buffer);
        }
        public static byte[] encodeDisconnectionRequest(DisconnectionRequest request)
        {
            return encodeUnencryptedRequest(request, RequestCodes.disconnect);
        }

        public static NotificationRequest decodeNotificationRequest(byte[] buffer)
        {
            return decodeUnencryptedRequest<NotificationRequest>(buffer);
        }
        public static byte[] encodeNotificationRequest(NotificationRequest request)
        {
            return encodeUnencryptedRequest(request, RequestCodes.notification);
        }


        public static BroadcastRequest decryptBroadcastRequest(byte[] buffer, byte[] key)
        {
            byte[] data = SharedPacketManipulation.trimRequestCode(buffer);
            string jsonString = decodeEncryptedString(data, key);

            try
            {
                return JsonSerializer.Deserialize<BroadcastRequest>(jsonString);
            }
            catch(Exception e)
            {
                return new BroadcastRequest() { userName = "WARNING:", message = jsonString };
            }
        }
        public static byte[] encryptBroadcastRequest(BroadcastRequest request, byte[] key)
        {
            string jsonString = JsonSerializer.Serialize(request);
            byte[] encryptedObject = encryptString(jsonString, key);

            byte[] returnValue = new byte[encryptedObject.Length + 1];
            returnValue[0] = RequestCodes.broadcastMessage;
            Array.Copy(encryptedObject, 0, returnValue, 1, encryptedObject.Length);

            return returnValue;
        }
    }
}
