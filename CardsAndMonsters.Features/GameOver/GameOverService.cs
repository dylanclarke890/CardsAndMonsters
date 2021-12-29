using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using System;

namespace CardsAndMonsters.Features.GameOver
{
    public class GameOverService : IGameOverService
    {
        private readonly IDuelLogService _duelLogService;

        public GameOverService(IDuelLogService duelLogService)
        {
            _duelLogService = duelLogService;
        }

        public Action<GameOverInfo> OnLoss { get; set; }

        public bool GameOver { get; set; }

        public void CheckForGameOver(Board board)
        {
            if (board.Player.OutOfHealth())
            {
                EndGame(board.Player, LossReason.NoHP);
                _duelLogService.AddNewEventLog(Event.GameEnded, board.Opponent);
            }
            if (board.Opponent.OutOfHealth())
            {
                EndGame(board.Opponent, LossReason.NoHP);
                _duelLogService.AddNewEventLog(Event.GameEnded, board.Player);
            }
        }

        public void EndGame(Duelist duelist, LossReason reason)
        {
            GameOverInfo gameOverInfo = new(duelist, reason, _duelLogService.GetEventLogs());
            GameOver = true;

            OnLoss.Invoke(gameOverInfo);
        }
    }
}
