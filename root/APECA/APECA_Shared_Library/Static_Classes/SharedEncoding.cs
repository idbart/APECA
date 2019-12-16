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

        public static ConnectionRequest decodeConnectionRequest(byte[] buffer)
        {
            byte[] data = SharedPacketManipulation.trimRequestCode(buffer);
            string jsonString = decodeString(data);

            return JsonSerializer.Deserialize<ConnectionRequest>(jsonString);
        }
        public static byte[] encodeConnectionRequest(ConnectionRequest request)
        {
            string jsonString = JsonSerializer.Serialize(request);
            byte[] encodedObject = encodeString(jsonString);

            byte[] returnValue = new byte[encodedObject.Length + 1];
            returnValue[0] = RequestCodes.connect;
            Array.Copy(encodedObject, 0, returnValue, 1, encodedObject.Length);

            return returnValue;
        }

        public static DisconnectionRequest decodeDisconnectionRequest(byte[] buffer)
        {
            byte[] data = SharedPacketManipulation.trimRequestCode(buffer);
            string jsonString = decodeString(data);

            return JsonSerializer.Deserialize<DisconnectionRequest>(jsonString);
        }
        public static byte[] encodeDisconnectionRequest(DisconnectionRequest request)
        {
            string jsonString = JsonSerializer.Serialize(request);
            byte[] encodedObject = encodeString(jsonString);

            byte[] returnValue = new byte[encodedObject.Length + 1];
            returnValue[0] = RequestCodes.disconnect;
            Array.Copy(encodedObject, 0, returnValue, 1, encodedObject.Length);

            return returnValue;
        }

        public static BrodcastRequest decryptBrodcastRequest(byte[] buffer, byte[] key)
        {
            byte[] data = SharedPacketManipulation.trimRequestCode(buffer);
            string jsonString = decodeEncryptedString(data, key);

            return JsonSerializer.Deserialize<BrodcastRequest>(jsonString);
        }
        public static byte[] encryptBrodcastRequest(BrodcastRequest request, byte[] key)
        {
            string jsonString = JsonSerializer.Serialize(request);
            byte[] encryptedObject = encryptString(jsonString, key);

            byte[] returnValue = new byte[encryptedObject.Length + 1];
            returnValue[0] = RequestCodes.brodcastMessage;
            Array.Copy(encryptedObject, 0, returnValue, 1, encryptedObject.Length);

            return returnValue;
        }
    }
}
