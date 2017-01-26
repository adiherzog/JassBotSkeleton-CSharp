using JassBot.Messages;
using System;
using JassBot.JassLogic;
using JassBot.Model;

namespace JassBot.Bot
{
    public class Game
    {
        private readonly string _playerName;
        private readonly string _sessionName;
        private readonly MessageSender _messageSender;
        private readonly bool _tournament;

        private readonly CardChooser _cardChooser = new CardChooser();
        private int _numberOfGamesPlayed = 0;
        private GameState _gameState = new GameState();

        public Game(string playerName, string sessionName, MessageSender messageSender, bool tournament)
        {
            Console.WriteLine(playerName + " created new Game");
            _playerName = playerName;
            _sessionName = sessionName;
            _messageSender = messageSender;
            _tournament = tournament;
        }

        public void ProcessMessage(IncommingMessage incommingMessage)
        {
            switch (incommingMessage.Type)
            {
                case MessageType.REQUEST_PLAYER_NAME:
                    _messageSender.SendMessage(CreatePlayerNameMessage(), _playerName);
                    break;
                case MessageType.REQUEST_SESSION_CHOICE:
                    _messageSender.SendMessage(CreateSessionChoiceMessage(), _playerName);
                    break;
                case MessageType.SESSION_JOINED:
                    // TODO
                    break;
                case MessageType.DEAL_CARDS:
                    _gameState.MyCards.UnionWith(incommingMessage.Cards);
                    break;
                case MessageType.REQUEST_TRUMPF:
                    var trumpf = new TrumpfChooser().ChooseTrumpf(_gameState.MyCards, incommingMessage.SchiebenAllowed);
                    if (!trumpf.Mode.Equals(TrumpfMode.SCHIEBE))
                    {
                        _gameState.DidNotSchiebe = true;
                    }
                    _messageSender.SendMessage(CreateChooseTrumpfMessage(trumpf), _playerName);
                    break;
                case MessageType.BROADCAST_TRUMPF:
                    _gameState.Trumpf = incommingMessage.Trumpf;
                    break;
                case MessageType.REQUEST_CARD:
                    _gameState.CardsOnTable = incommingMessage.CardsOnTable;
                    var card = _cardChooser.ChooseCard(_gameState);
                    _gameState.MyCards.Remove(card);
                    _messageSender.SendMessage(CreateChooseCardMessage(card), _playerName);
                    break;
                case MessageType.BROADCAST_STICH:
                    Console.WriteLine(_playerName + ": " + incommingMessage.Stich);
                    _gameState.StartNextRound();
                    break;
                // One game finished (9 rounds)
                case MessageType.BROADCAST_GAME_FINISHED:
                    _gameState.ResetAfterGameRound();
                    break;
                case MessageType.BROADCAST_WINNER_TEAM:
                    Console.Write("Game #" + _numberOfGamesPlayed + "Finished! Press any key to play another game.");
                    _gameState = new GameState();
                    _numberOfGamesPlayed++;
                    break;
            }
        }

        private OutgoingMessage CreatePlayerNameMessage()
        {
            var outgoingMessage = new OutgoingMessage(MessageType.CHOOSE_PLAYER_NAME) {Data = _playerName};
            return outgoingMessage;
        }

        private OutgoingMessage CreateSessionChoiceMessage()
        {
            var outgoingMessage = new OutgoingMessage(MessageType.CHOOSE_SESSION)
            {
                Data = CreateSessionChoiceData()
            };
            return outgoingMessage;
        }

        private SessionChoiceData CreateSessionChoiceData()
        {
            if (_tournament)
            {
                return new SessionChoiceData(SessionChoiceType.AUTOJOIN, null, SessionType.TOURNAMENT);
            }
            else
            {
                return new SessionChoiceData(SessionChoiceType.AUTOJOIN, _sessionName, SessionType.SINGLE_GAME);
            }
        }

        private static OutgoingMessage CreateChooseTrumpfMessage(Trumpf trumpf)
        {
            return new OutgoingMessage(MessageType.CHOOSE_TRUMPF) {Data = trumpf};
        }

        private static OutgoingMessage CreateChooseCardMessage(Card card)
        {
            return new OutgoingMessage(MessageType.CHOOSE_CARD) {Data = card};
        }
    }
}
