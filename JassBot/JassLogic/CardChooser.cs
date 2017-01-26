using System.Linq;
using JassBot.Util;
using JassBot.Model;

namespace JassBot.JassLogic
{
    public class CardChooser
    {
        public Card ChooseCard(GameState gameState)
        {
            Preconditions.CheckNotNull(gameState);
            gameState.CheckIsValid();

            return gameState.GetAllowedCardsToPlay().First();
        }
    }
}
