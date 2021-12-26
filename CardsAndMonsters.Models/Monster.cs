using System;

namespace CardsAndMonsters.Models
{
    public class Monster : BaseCard
    {
        public Monster(decimal attack, decimal defense)
        {
            Id = Guid.NewGuid();
            Attack = attack;
            Defense = defense;
            AttacksPerTurn = 1;
        }

        public decimal Attack { get; set; }

        public decimal Defense { get; set; }

        public int AttacksPerTurn { get; set; }
    }
}
