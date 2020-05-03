using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using APECA_Shared_Library;

namespace APECA_Tests
{
    [TestClass]
    public class SharedPacketTranslationTests
    {
        [TestMethod]
        public void isConnectionRequest_givenConnectionRequest_returnsTrue()
        {
            byte[] buffer = new byte[1024];
            buffer[0] = RequestCodes.connect;

            bool result = SharedPacketTranslation.isConnectionRequest(buffer);

            Assert.IsTrue(result);
        }
        [TestMethod] 
        public void isConnectionRequest_notGivenConnectionRequest_returnsFalse()
        {
            byte[] buffer = new byte[1024];
            buffer[0] = RequestCodes.disconnect;

            bool result = SharedPacketTranslation.isConnectionRequest(buffer);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void isDisconnectRequest_givenDisconnectRequest_returnsTrue()
        {
            byte[] buffer = new byte[1024];
            buffer[0] = RequestCodes.disconnect;

            bool result = SharedPacketTranslation.isDisconnectRequest(buffer);

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void isDisconnectRequest_notGivenDisconnectRequest_returnsFalse()
        {
            byte[] buffer = new byte[1024];
            buffer[0] = RequestCodes.connect;

            bool result = SharedPacketTranslation.isDisconnectRequest(buffer);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void isBrodcastRequest_givenBrodcastRequest_returnsTrue()
        {
            byte[] buffer = new byte[1024];
            buffer[0] = RequestCodes.broadcastMessage;

            bool result = SharedPacketTranslation.isBrodcastRequest(buffer);

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void isBrodcastRequest_notGivenBrodcastRequest_returnsFalse()
        {
            byte[] buffer = new byte[1024];
            buffer[0] = RequestCodes.connect;

            bool result = SharedPacketTranslation.isBrodcastRequest(buffer);

            Assert.IsFalse(result);
        }
    }
}
