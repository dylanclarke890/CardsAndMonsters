using CardsAndMonsters.Models.Enums;

namespace CardsAndMonsters.Models
{
    public class GameOverInfo
    {
        public GameOverInfo(Player player, LossReason lossReason)
        {
            LosingPlayer = player;
            LossReason = lossReason;
        }

        public Player LosingPlayer { get; set; }

        public LossReason LossReason { get; set; }
    }
}
