using CardsAndMonsters.Models.Enums;

namespace CardsAndMonsters.Models
{
    public class BattleInfo
    {
        public Player AttackingPlayer { get; set; }

        public Monster AttackingMonster { get; set; }

        public BattleTarget Target { get; set; }

        public Player DefendingPlayer { get; set; }

        public Monster TargetMonster { get; set; }

        public Board Board { get; set; }
    }
}
