using System;

namespace JassBot.Messages
{
    public class IllegalMessageException : ArgumentException
    {
        public string MessageFromServer { get; }

        public IllegalMessageException(string message, string messageFromServer) : base(message)
        {
            MessageFromServer = messageFromServer;
        }
    }
}
