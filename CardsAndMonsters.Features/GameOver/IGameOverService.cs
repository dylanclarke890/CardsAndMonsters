using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using System;

namespace CardsAndMonsters.Features.GameOver
{
    public interface IGameOverService
    {
        Action<GameOverInfo> OnLoss { get; set; }

        void CheckForGameOver(Board board);
        void EndGame(Duelist player, LossReason reason);
    }
}