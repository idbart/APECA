using System;
using System.Collections.Generic;
using System.Text;

namespace APECA_Shared_Library
{
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
}
