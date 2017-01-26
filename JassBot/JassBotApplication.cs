using JassBot.Bot;
using System;
using System.Threading;
using JassBot.Messages;

namespace JassBot
{
    /// <summary>
    /// This is a Jass (swiss card game) bot implementation that plays the game
    /// over an instance of the https://github.com/webplatformz/challenge Jass server.
    /// </summary>
    /// 
    /// @author Adrian Herzog (https://github.com/adiherzog)
    internal class JassBotApplication
    {
        private const string DefaultServerUri = "ws://localhost:3000";
        private const bool Tournament = false;
        private const string SessionName = "showdown"; // not relevant for tournament
        private const int NumberOfBotPlayers = 4;
        private const string BotName = "CS-Skeleton"; // will be used as prefix for bot in front of player names
        private static readonly string[] PlayerNames = {"moss", "roy", "jen", "douglas"};

        private static void Main(string[] args)
        {
            Console.WriteLine("Starting " + NumberOfBotPlayers + " Jass Bots");
            var countDown = new CountdownEvent(NumberOfBotPlayers);
            var serverUri = (args.Length > 0) ? args[0] : DefaultServerUri;

            try
            {
                for (var i = 0; i < NumberOfBotPlayers; i++)
                {
                    // ReSharper disable once ObjectCreationAsStatement
                    CreateBotPlayer(PlayerNames[i], countDown, serverUri);
                }
            } catch(Exception e)
            {
                Console.WriteLine("Exception while playing: " + e);
            }

            countDown.Wait();
            Console.ReadKey();
        }

        /// <summary>
        /// Creates one bot jass player. This program can simulate multiple players.
        /// </summary>
        private static void CreateBotPlayer(string playerName, CountdownEvent countDown, string ServerUri)
        {
            var name = BotName + "-" + playerName;
            Console.WriteLine("Creating bot player " + name + " for session " + SessionName);

            var endpoint = new WebSocketListener(name, SessionName, countDown, Tournament);

            new WebSocketClient().ConnectToServer(endpoint, new Uri(ServerUri));
        }
    }
}
