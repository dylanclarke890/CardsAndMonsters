using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;

namespace CardsAndMonsters.Features.TurnPhase
{
    public class PhaseService : IPhaseService
    {
        public void EnterPhase(Phase phase, Board board)
        {
            board.CurrentTurn.Phase = phase;
        }
    }
}
