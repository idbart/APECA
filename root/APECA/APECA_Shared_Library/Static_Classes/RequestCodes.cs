using System;
using System.Collections.Generic;
using System.Text;

namespace APECA_Shared_Library
{
    public static class RequestCodes
    {
        public static readonly byte connect = 1;
        public static readonly byte disconnect = 2;
        public static readonly byte broadcastMessage = 3;
        public static readonly byte notification = 4;
    }
}
