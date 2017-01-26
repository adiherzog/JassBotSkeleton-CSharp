using System;
using JassBot.Messages;
using JassBot.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JassBotTest.Messages
{
    [TestClass]
    public class MessageReaderTest
    {
        private readonly MessageReader _messageReader = new MessageReader();

        [TestMethod]
        public void requestPlayerNameMessage_thatIsValid_isReadCorrectly()
        {
            const string messageFromServer = "{\"type\":\"REQUEST_PLAYER_NAME\"}";

            var message = _messageReader.Read(messageFromServer);

            Assert.IsNotNull(message);
            Assert.AreEqual(MessageType.REQUEST_PLAYER_NAME, message.Type);
        }

        [TestMethod]
        public void broadcastTrumpf_thatIsValid_isReadCorrectly()
        {
            const string messageFromServer = @"{
                    ""type"" : ""BROADCAST_TRUMPF"",
                    ""data"" : {
                        ""mode"" : ""TRUMPF"",
                        ""trumpfColor"" : ""SPADES""
                    }
                }";

            var message = _messageReader.Read(messageFromServer);

            Assert.IsNotNull(message);
            Assert.AreEqual(MessageType.BROADCAST_TRUMPF, message.Type);
            Assert.AreEqual(TrumpfMode.TRUMPF, message.Trumpf.Mode);
            Assert.AreEqual(Suit.SPADES, message.Trumpf.Suit);
        }

        [TestMethod]
        public void messageWithoutType_throwsException()
        {
            const string messageFromServer = "{\"somethingElse\":\"bla\"}";

            try
            {
                var message = _messageReader.Read(messageFromServer);
            }
            catch (IllegalMessageException e)
            {
                Assert.AreEqual(messageFromServer, e.MessageFromServer);
                Assert.AreEqual("Field \"type\" is missing.", e.Message);
            }
        }
    }
}
