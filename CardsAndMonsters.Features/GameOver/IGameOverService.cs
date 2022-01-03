using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.GameOver
{
    public interface IGameOverService
    {
        Action<GameOverInfo> OnLoss { get; set; }
        bool GameOver { get; set; }

        Task CheckForGameOver(Board board);
        void ClearGameOverInfo();
        Task EndGame(Duelist player, LossReason reason);
    }
}