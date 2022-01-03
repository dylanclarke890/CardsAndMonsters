using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Cards;
using CardsAndMonsters.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Game
{
    public interface IGameService
    {
        Board Board { get; set; }
        Action OnAction { get; set; }

        Task Attack(BattleInfo battleInfo);
        Task<bool> CheckForExistingGame();
        Task EnterPhase(Phase phase);
        Task EndTurn();
        void PlayCard(BaseCard card);
        Task PlayMonster(FieldPosition position);
        void PlayMonster(Monster monster);
        Task NewGame();
        Task ResumeGame();
        Task ClearGame();
        Task SwitchPosition(Monster monster);
    }
}