using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace APECA_Shared_Library
{
    public static class Cipher
    {
        private static AesCryptoServiceProvider cipher = new AesCryptoServiceProvider() { KeySize = 128, BlockSize = 128, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 };

        public static byte[] generateKey_128Bit()
        {
            byte[] key = new byte[16];
            Random rand = new Random();
            rand.NextBytes(key);

            return key;
        }

        public static byte[] encrypt(byte[] data, byte[] key)
        {
            cipher.Key = key;
            cipher.GenerateIV();

            byte[] IV = cipher.IV;
            ICryptoTransform encryptor = cipher.CreateEncryptor();
            byte[] cipherText = encryptor.TransformFinalBlock(data, 0, data.Length);
            byte[] dataReturn = new byte[16 + cipherText.Length];

            Array.Copy(cipherText, 0, dataReturn, 16, cipherText.Length);
            Array.Copy(IV, 0, dataReturn, 0, IV.Length);

            return dataReturn;
        }
        public static byte[] decrypt(byte[] data, byte[] key)
        {
            byte[] dataReturn = new byte[data.Length - 16];

            cipher.Key = key;
            byte[] IV = new byte[16];
            Array.Copy(data, 0, IV, 0, 16);
            cipher.IV = IV;

            ICryptoTransform decryptor = cipher.CreateDecryptor();
            try
            {
                dataReturn = decryptor.TransformFinalBlock(data, 16, dataReturn.Length);
            }
            catch (Exception exe)
            {
                return data;
            }

            return dataReturn;
        }
    }
}
