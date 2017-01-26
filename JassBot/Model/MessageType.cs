using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace JassBot.Model
{
    public enum MessageType
    {
        REQUEST_PLAYER_NAME,
        CHOOSE_PLAYER_NAME,
        SESSION_JOINED,
        DEAL_CARDS,
        REQUEST_TRUMPF,
        CHOOSE_TRUMPF,
        BROADCAST_TRUMPF,
        BROADCAST_STICH,
        BROADCAST_WINNER_TEAM,
        BROADCAST_GAME_FINISHED,
        BROADCAST_SESSION_JOINED,
        BROADCAST_TEAMS,
        PLAYED_CARDS,
        REQUEST_CARD,
        CHOOSE_CARD,
        REJECT_CARD,
        REQUEST_SESSION_CHOICE,
        CHOOSE_SESSION,
        BAD_MESSAGE,
        BROADCAST_TOURNAMENT_RANKING_TABLE,
        START_TOURNAMENT,
        BROADCAST_TOURNAMENT_STARTED
    }
}
