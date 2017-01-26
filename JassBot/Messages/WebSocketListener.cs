using System;
using System.Threading;
using JassBot.Bot;

namespace JassBot.Messages
{
    // Reacts to certain events coming from the websocket
    public class WebSocketListener
    {
        private readonly MessageReader _messageReader = new MessageReader();
        private readonly string _playerName;
        private readonly string _sessionName;
        private readonly CountdownEvent _countDown;
        private readonly bool _tournament;

        public Game Game { get; private set; }

        public WebSocketListener(string playerName, string sessionName, CountdownEvent countDown, bool tournament)
        {
            _playerName = playerName;
            _sessionName = sessionName;
            _countDown = countDown;
            _tournament = tournament;
        }

        // Web socket connection opened
        public void Open(WebSocketClient client)
        {
            Console.WriteLine(_playerName + ": Opened connection to server.");
            Game = new Game(_playerName, _sessionName, new MessageSender(client), _tournament);
        }

        /**
         * Receive a message. Possible messages see:<br>
         * https://github.com/webplatformz/challenge/wiki/Sample-Output<br>
         * https://github.com/webplatformz/challenge/blob/master/shared/messages/messages.js
         */
        public void Message(string messageFromServer)
        {
            Console.WriteLine("<-- " + _playerName + " receives: " + messageFromServer);
            Game.ProcessMessage(_messageReader.Read(messageFromServer));
        }

        // Web socket connection closed
        public void OnClose()
        {
            Console.WriteLine(_playerName + ": Session closed");
            _countDown.Signal();
        }

        public void OnError(Exception e)
        {
            Console.WriteLine("WebSocked error: " + e);
        }
    }
}
