using CardsAndMonsters.Features.Logging;
using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.TurnPhase
{
    public class PhaseService : IPhaseService
    {
        private readonly IDuelLogService _duelLogService;
        private readonly int _animationDelay = 1;

        public PhaseService(IDuelLogService duelLogService)
        {
            _duelLogService = duelLogService;
        }

        public Action PhaseChanged { get; set; }

        public Action<int, Phase> OnShow { get; set; }

        public Action OnHide { get; set; }

        public async Task EnterPhase(Phase phase, Board board)
        {
            board.CurrentTurn.Phase = phase;
            PhaseChanged?.Invoke();
            _duelLogService.AddNewEventLog(Event.PhaseChange, board.CurrentTurn.Duelist);
            OnShow?.Invoke(_animationDelay, phase);
            await Task.Delay(_animationDelay * 1000);
            OnHide?.Invoke();
        }
    }
}
