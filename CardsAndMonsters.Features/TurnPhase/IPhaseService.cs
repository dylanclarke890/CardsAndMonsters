using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;

namespace CardsAndMonsters.Features.TurnPhase
{
    public interface IPhaseService
    {
        void EnterPhase(Phase phase, Board board);
    }
}