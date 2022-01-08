using System;

namespace CardsAndMonsters.Models.Cards
{
    public class Monster : BaseCard
    {
        public Monster(decimal attack, decimal defense)
        {
            Attack = attack;
            Defense = defense;
            AttacksPerTurn = 1;
        }

        public decimal Attack { get; set; }

        public decimal Defense { get; set; }

        public int AttacksPerTurn { get; set; }
    }
}
