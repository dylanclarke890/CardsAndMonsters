using CardsAndMonsters.Core;
using CardsAndMonsters.Models.Base;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Turns;
using System.Collections.Generic;
using System.Linq;

namespace CardsAndMonsters.Models
{
    public class Duelist : BaseModel
    {
        public Duelist()
        {
            CurrentHand = new List<BaseCard>();
            Deck = new List<BaseCard>();
        }

        public Duelist(string name)
        {
            Name = name;
            HP = 4000;
            CardLimit = AppConstants.HandSize;
            CurrentHand = new List<BaseCard>();
            Deck = new List<BaseCard>();
        }

        public string Name { get; set; }

        public decimal HP { get; set; }

        public int CardLimit { get; set; }

        public IList<BaseCard> CurrentHand { get; set; }

        public IList<BaseCard> Deck {get; set;}

        public bool OutOfHealth()
        {
            return HP <= 0;
        }

        public bool DrawCard()
        {
            if (!Deck.Any())
            {
                return false;
            }

            CurrentHand.Add(Deck[0]);
            Deck.Remove(Deck[0]);

            return true;
        }

        public void PlayMonster(Monster monster, Board board, TurnState turn)
        {
            CurrentHand.Remove(monster);

            if (turn.Duelist.Equals(board.Player))
            {
                board.PlayerField.Monsters.Add(monster);
            }
            else
            {
                board.OpponentField.Monsters.Add(monster);
            }
            turn.SummonedThisTurn.Add(monster);
            turn.MonsterState[monster.Id] = new() 
            { 
                AbleToSwitch = false, 
                TimesAttacked = 0, 
                Monster = monster
            };
        }

        public void TakeDamage(decimal amount)
        {
            HP -= amount;
            HP = HP < 0 ? 0 : HP;
        }
    }
}
