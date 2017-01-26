using System;
using JassBot.Bot;
using JassBot.Util;
using Newtonsoft.Json;

namespace JassBot.Messages
{
    public class MessageSender
    {
        private readonly WebSocketClient _webSocket;

        public MessageSender(WebSocketClient webSocket)
        {
            Preconditions.CheckNotNull(webSocket);
            _webSocket = webSocket;
        }

        public void SendMessage(OutgoingMessage outgoingMessage, string playerName)
        {
            var messageAsJson = JsonConvert.SerializeObject(outgoingMessage);
            this._webSocket.Send(messageAsJson, playerName);
        }
    }
}
