using CardsAndMonsters.Core;
using CardsAndMonsters.Models.Enums;
using System.Collections.Generic;
using System.Diagnostics;

namespace CardsAndMonsters.Models
{
    public class Board : BaseModel
    {
        public Board(Player player, Player opponent)
        {
            Player = player;
            Opponent = opponent;
            Turns = new Dictionary<int, TurnState>();
            OpponentMonsters = new List<Monster>();
            PlayerMonsters = new List<Monster>();
        }

        public TurnState CurrentTurn { get; set; }

        public IDictionary<int, TurnState> Turns { get; set; }

        public Player Player { get; set; }
        
        public Player Opponent { get; set; }
        
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
