using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using System;

namespace CardsAndMonsters.Features.GameOver
{
    public class GameOverService : IGameOverService
    {
        public Action<GameOverInfo> OnLoss { get; set; }

        public bool GameOver { get; set; }

        public void CheckForGameOver(Board board)
        {
            if (board.Player.OutOfHealth())
            {
                EndGame(board.Player, LossReason.NoHP);
            }
            if (board.Opponent.OutOfHealth())
            {
                EndGame(board.Opponent, LossReason.NoHP);
            }
        }

        public void EndGame(Duelist player, LossReason reason)
        {
            GameOverInfo gameOverInfo = new(player, reason);
            GameOver = true;
            OnLoss.Invoke(gameOverInfo);
        }
    }
}
