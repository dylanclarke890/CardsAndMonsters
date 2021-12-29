using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.TurnPhase
{
    public class PhaseService : IPhaseService
    {
        private readonly int _animationDelay = 4;

        public Action<int, Phase> OnShow { get; set; }

        public Action OnHide { get; set; }

        public async Task EnterPhase(Phase phase, Board board)
        {
            board.CurrentTurn.Phase = phase;
            OnShow?.Invoke(_animationDelay, phase);
            await Task.Delay(_animationDelay * 1000);
            OnHide?.Invoke();
        }
    }
}
