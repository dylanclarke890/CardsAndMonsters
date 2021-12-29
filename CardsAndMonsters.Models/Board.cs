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
            Opponent = opponent;
            TurnCount = 0;
            Turns = new Dictionary<int, TurnState>();
            OpponentMonsters = new List<Monster>();
            PlayerMonsters = new List<Monster>();
        }

        public int TurnCount { get; set; }

        public TurnState CurrentTurn { get; set; }

        public IDictionary<int, TurnState> Turns { get; set; }

        public Duelist Player { get; set; }
        
        public Duelist Opponent { get; set; }
        
        public IList<Monster> OpponentMonsters { get; set; }
        
        public IList<Monster> PlayerMonsters { get; set; }

        public bool AbleToPlayMonster(Monster monster)
        {
            return CurrentTurn?.Phase is Phase.Main && (bool)CurrentTurn?.Player.Equals(Player) 
                && (bool)!CurrentTurn?.NormalSummonLimitReached() && PlayerMonsters.Count < AppConstants.FieldSize 
                && Player.CurrentHand.Contains(monster);
        }
    }
}
