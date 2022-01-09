using CardsAndMonsters.Core.Exceptions;
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
        private readonly IBoardManagementService _boardManagementService;
        private readonly IDuelLogService _duelLogService;

        public GameOverService(IBoardManagementService boardManagementService, IDuelLogService duelLogService)
        {
            _boardManagementService = boardManagementService;
            _duelLogService = duelLogService;
        }

        public Action<GameOverInfo> OnLoss { get; set; }

        public bool GameOver { get; set; }

        public async Task CheckForGameOver(Board board)
        {
            if (board == null)
            {
                throw new GameArgumentException<Board>(nameof(board), board);
            }

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
            if (duelist == null)
            {
                throw new GameArgumentException<GameOverInfo>(nameof(duelist), duelist);
            }

            GameOverInfo gameOverInfo = new(duelist, reason, _duelLogService.GetEventLogs());
            GameOver = true;

            await _boardManagementService.Delete();
            OnLoss?.Invoke(gameOverInfo);
        }

        public void ClearGameOverInfo()
        {
            GameOver = false;
            GameOverInfo gameOverInfo = null;
            OnLoss?.Invoke(gameOverInfo);
        }
    }
}
