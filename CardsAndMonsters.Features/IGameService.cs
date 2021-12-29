using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features
{
    public interface IGameService
    {
        Board Board { get; set; }
        bool ChoosingFieldPosition { get; set; }
        Action OnAction { get; set; }
        BaseCard PendingPlacement { get; set; }

        void Attack(BattleInfo battleInfo);
        Task EnterPhase(Phase phase);
        Task EndTurn();
        void PlayCard(BaseCard card);
        void PlayMonster(FieldPosition position);
        void PlayMonster(Monster monster);
        Task StartGame();
        void SwitchPosition(Monster monster);
    }
}