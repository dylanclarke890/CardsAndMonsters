using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;

namespace CardsAndMonsters.Models
{
    public class BattleInfo
    {
        public Duelist AttackingPlayer { get; set; }

        public Monster AttackingMonster { get; set; }

        public BattleTarget Target { get; set; }

        public Duelist DefendingPlayer { get; set; }

        public Monster TargetMonster { get; set; }

        public bool Successful { get; set; }

        public Board Board { get; set; }
    }
}
