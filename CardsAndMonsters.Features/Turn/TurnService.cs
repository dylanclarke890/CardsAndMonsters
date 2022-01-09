using CardsAndMonsters.Core.Exceptions;
using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Turn
{
    public class TurnService : ITurnService
    {
        private readonly IDuelLogService _duelLogService;
        private readonly IGameOverService _gameOverService;
        private readonly IPhaseService _phaseService;

        public TurnService(IDuelLogService duelLogService, IGameOverService gameOverService,
            IPhaseService phaseService)
        {
            _duelLogService = duelLogService;
            _gameOverService = gameOverService;
            _phaseService = phaseService;
        }

        public async Task StartTurn(Duelist duelist, bool drawCard, Board board)
        {
            if (duelist == null)
            {
                throw new GameArgumentException<Board>(nameof(duelist), duelist);
            }
            if (board == null)
            {
                throw new GameArgumentException<Board>(nameof(board), board);
            }

            board.CurrentTurn = new(board.Player.Equals(duelist) ? board.PlayerField.Monsters : board.OpponentField.Monsters, duelist);
            await _phaseService.EnterPhase(Phase.Standby, board);

            if (drawCard)
            {
                var success = duelist.DrawCard();
                board.CurrentTurn.CardsDrawn++;
                _duelLogService.AddNewEventLog(Event.DrawCard, duelist);

                if (!success)
                {
                    await _gameOverService.EndGame(duelist, LossReason.DeckOut);
                    return;
                }
            }

            await _phaseService.EnterPhase(Phase.Main, board);
        }

        public async Task ResumeTurn(Board board)
        {
            if (board == null)
            {
                throw new GameArgumentException<Board>(nameof(board), board);
            }

            // Re-enter the last phase
            await _phaseService.EnterPhase(board.CurrentTurn.Phase, board);
        }

        public async Task EndTurn(Board board)
        {
            if (board == null)
            {
                throw new GameArgumentException<Board>(nameof(board), board);
            }

            await _phaseService.EnterPhase(Phase.End, board);

            _duelLogService.AddNewEventLog(Event.TurnChange, board.CurrentTurn.Duelist);
            board.Turns[board.TurnCount] = board.CurrentTurn;
            board.TurnCount++;

            await StartTurn(board.Turns.Last().Value.Duelist.Equals(board.Player) ? board.Opponent : board.Player, true, board);
        }
    }
}
