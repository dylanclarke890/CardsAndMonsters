using System;

namespace CardsAndMonsters.Models
{
    public class Monster : BaseCard
    {
        public Monster()
        {
            Id = Guid.NewGuid();
        }

        public Monster(decimal attack, decimal defense)
        {
            Id = Guid.NewGuid();
            Attack = attack;
            Defense = defense;
        }

        public decimal Attack { get; set; }
        public decimal Defense { get; set; }
    }
}
