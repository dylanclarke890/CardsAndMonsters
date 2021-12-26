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
            SummonedThisTurn = new List<Monster>();
            MonsterState = new Dictionary<Guid, MonsterTurnState>();
        }

        public TurnState(IList<Monster> monsters, Player player)
        {
            NormalSummonLimit = 1;
            SummonedThisTurn = new List<Monster>();
            MonsterState = new Dictionary<Guid, MonsterTurnState>();
            Player = player;

            foreach (var monster in monsters)
            {
                MonsterState.Add(monster.Id, new() 
                {
                    Monster = monster,
                    AbleToSwitch = true,
                    TimesAttacked = 0
                });
            }
        }

        /// <summary>
        /// Who's turn is it?
        /// </summary>
        public Player Player { get; set; }

        public Phase Phase { get; set; }

        public int NormalSummonLimit { get; set; }

        public IList<Monster> SummonedThisTurn { get; set; }

        public IDictionary<Guid, MonsterTurnState> MonsterState { get; set; }

        public bool NormalSummonLimitReached()
        {
            return SummonedThisTurn.Count == NormalSummonLimit;
        }

        public bool IsCurrentPlayer(Player player)
        {
            return Player == player;
        }
    }
}
