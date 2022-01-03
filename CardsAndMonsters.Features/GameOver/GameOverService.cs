using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.Storage;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.GameOver
{
    public class GameOverService : IGameOverService
    {
        private readonly IDuelLogService _duelLogService;
        private readonly IBoardManagementService _boardManagementService;

        public GameOverService(IDuelLogService duelLogService,
            IBoardManagementService boardManagementService)
        {
            _duelLogService = duelLogService;
            _boardManagementService = boardManagementService;
        }

        public Action<GameOverInfo> OnLoss { get; set; }

        public bool GameOver { get; set; }

        public async Task CheckForGameOver(Board board)
        {
            if (board.Player.OutOfHealth())
            {
                await EndGame(board.Player, LossReason.NoHP);
                _duelLogService.AddNewEventLog(Event.GameEnded, board.Opponent);
            }
            if (board.Opponent.OutOfHealth())
            {
                await EndGame(board.Opponent, LossReason.NoHP);
                _duelLogService.AddNewEventLog(Event.GameEnded, board.Player);
            }
        }

        public async Task EndGame(Duelist duelist, LossReason reason)
        {
            GameOverInfo gameOverInfo = new(duelist, reason, _duelLogService.GetEventLogs());
            GameOver = true;

            await _boardManagementService.Delete();
            OnLoss.Invoke(gameOverInfo);
        }

        public void ClearGameOverInfo()
        {
            GameOver = false;
            GameOverInfo gameOverInfo = null;
            OnLoss?.Invoke(gameOverInfo);
        }
    }
}
