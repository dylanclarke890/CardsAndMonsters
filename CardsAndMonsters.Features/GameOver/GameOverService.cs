using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using System;

namespace CardsAndMonsters.Features.GameOver
{
    public class GameOverService
    {
        public Action<GameOverInfo> OnLoss;

        public bool GameOver = false;

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

        public void EndGame(Player player, LossReason reason)
        {
            GameOverInfo gameOverInfo = new(player, reason);
            GameOver = true;
            OnLoss.Invoke(gameOverInfo);
        }
    }
}
