using CardsAndMonsters.Models.Enums;
using System;
using System.Collections.Generic;

namespace CardsAndMonsters.Models
{
    public class TurnState : BaseModel
    {
        public TurnState(Player player)
        {
            Player = player;
            NormalSummonLimit = 1;
            MonstersInPlay = new List<Monster>();
            AbleToSwitch = new Dictionary<Guid, bool>();
        }

        public TurnState(IList<Monster> monsters)
        {
            NormalSummonLimit = 1;
            MonstersInPlay = monsters;
            AbleToSwitch = new Dictionary<Guid, bool>();

            foreach (var monster in monsters)
            {
                AbleToSwitch.Add(monster.Id, true);
            }
        }

        /// <summary>
        /// Who's turn is it?
        /// </summary>
        public Player Player { get; set; }

        public Phase Phase { get; set; }

        public int NormalSummonLimit { get; set; }

        public IList<Monster> MonstersInPlay { get; set; }

        public IDictionary<Guid, bool> AbleToSwitch { get; set; }

        public bool NormalSummonLimitReached()
        {
            return MonstersInPlay.Count == NormalSummonLimit;
        }

        public bool IsCurrentPlayer(Player player)
        {
            return Player == player;
        }
    }
}
