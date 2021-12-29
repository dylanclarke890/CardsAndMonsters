using CardsAndMonsters.Models.Base;
using CardsAndMonsters.Models.Enums;

namespace CardsAndMonsters.Models
{
    public class GameOverInfo : BaseModel
    {
        public GameOverInfo(Duelist player, LossReason lossReason)
        {
            LosingPlayer = player;
            LossReason = lossReason;
        }

        public Duelist LosingPlayer { get; set; }

        public LossReason LossReason { get; set; }
    }
}
