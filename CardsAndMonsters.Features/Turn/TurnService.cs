using CardsAndMonsters.Features.GameOver;
using CardsAndMonsters.Features.TurnPhase;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using System.Linq;

namespace CardsAndMonsters.Features.Turn
{
    public class TurnService : ITurnService
    {
        private readonly IPhaseService _phaseService;
        private readonly IGameOverService _gameOverService;

        public TurnService(IPhaseService phaseService,
            IGameOverService gameOverService)
        {
            _phaseService = phaseService;
            _gameOverService = gameOverService;
        }

        public void StartTurn(Duelist player, bool drawCard, Board board)
        {
            bool isPlayer = board.Player.Equals(player);
            board.CurrentTurn = new(isPlayer ? board.PlayerField.Monsters : board.OpponentField.Monsters, player);

            if (drawCard)
            {
                var success = player.DrawCard();
                if (!success)
                {
                    _gameOverService.EndGame(player, LossReason.DeckOut);
                    return;
                }
            }

            _phaseService.EnterPhase(Phase.Main, board);
        }


        public void EndTurn(Board board)
        {
            _phaseService.EnterPhase(Phase.End, board);
            board.Turns[board.TurnCount] = board.CurrentTurn;
            board.TurnCount++;
            _phaseService.EnterPhase(Phase.Standby, board);
            StartTurn(board.Turns.Last().Value.Player.Equals(board.Player) ? board.Opponent : board.Player, true, board);
        }
    }
}
