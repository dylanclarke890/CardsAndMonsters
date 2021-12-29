using CardsAndMonsters.Core;
using CardsAndMonsters.Models.Base;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using CardsAndMonsters.Models.Turns;
using System.Collections.Generic;

namespace CardsAndMonsters.Models
{
    public class Board : BaseModel
    {
        public Board(Duelist player, Duelist opponent)
        {
            Player = player;
            PlayerField = new(player);
            Opponent = opponent;
            OpponentField = new(opponent);
            TurnCount = 0;
            Turns = new Dictionary<int, TurnState>();
        }

        public int TurnCount { get; set; }

        public TurnState CurrentTurn { get; set; }

        public IDictionary<int, TurnState> Turns { get; set; }

        public Duelist Player { get; set; }

        public Duelist Opponent { get; set; }

        public FieldState PlayerField { get; set; }

        public FieldState OpponentField { get; set; }

        public bool AbleToPlayMonster(Monster monster)
        {
            return CurrentTurn?.Phase is Phase.Main && (bool)CurrentTurn?.Duelist.Equals(PlayerField.Duelist) 
                && (bool)!CurrentTurn?.NormalSummonLimitReached() && PlayerField.Monsters.Count < AppConstants.FieldSize 
                && PlayerField.Duelist.CurrentHand.Contains(monster);
        }
    }
}
