using CardsAndMonsters.Models.Base;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System;
using System.Collections.Generic;

namespace CardsAndMonsters.Models.Turns
{
    public class TurnState : BaseModel
    {
        public TurnState()
        {
            Battles = new List<BattleInfo>();
            SummonedThisTurn = new List<Monster>();
            MonsterState = new Dictionary<Guid, MonsterTurnState>();
        }

        public TurnState(Duelist duelist)
        {
            Duelist = duelist;
            Battles = new List<BattleInfo>();
            NormalSummonLimit = 1;
            SummonedThisTurn = new List<Monster>();
            MonsterState = new Dictionary<Guid, MonsterTurnState>();
        }

        public TurnState(IList<Monster> monsters, Duelist duelist)
        {
            NormalSummonLimit = 1;
            Battles = new List<BattleInfo>();
            SummonedThisTurn = new List<Monster>();
            MonsterState = new Dictionary<Guid, MonsterTurnState>();
            Duelist = duelist;

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

        public Duelist Duelist { get; set; }

        public Phase Phase { get; set; }

        public int NormalSummonLimit { get; set; }

        public IList<Monster> SummonedThisTurn { get; set; }

        public IDictionary<Guid, MonsterTurnState> MonsterState { get; set; }

        public IList<BattleInfo> Battles { get; set; }

        public int CardsDrawn { get; set; }

        public bool NormalSummonLimitReached()
        {
            return SummonedThisTurn.Count == NormalSummonLimit;
        }

        public bool AbleToSwitch(Guid monsterId, Duelist duelist)
        {
            return Phase is Phase.Main && MonsterState.TryGetValue(monsterId, out var result)
                && result.AbleToSwitch && Duelist.Equals(duelist);
        }

        public bool AbleToBattle(Guid monsterId, Duelist duelist, bool declaringAttack)
        {
            return Phase is Phase.Battle && MonsterState.TryGetValue(monsterId, out var result)
                && result.TimesAttacked < result.Monster.AttacksPerTurn && result.Monster.FieldPosition is FieldPosition.VerticalUp
                && Duelist.Equals(duelist) && !declaringAttack;
        }

        public bool AbleToAttack(Guid monsterId, Duelist duelist, bool declaringAttack)
        {
            return Phase is Phase.Battle && !MonsterState.TryGetValue(monsterId, out _) && Duelist.Equals(duelist)
                && declaringAttack;
        }
    }
}
