using CardsAndMonsters.Models;
using CardsAndMonsters.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.TurnPhase
{
    public interface IPhaseService
    {
        Action PhaseChanged { get; set; }
        Action<int, Phase> OnShow { get; set; }
        Action OnHide { get; set; }
        bool ChangingPhase { get; set; }

        Task EnterPhase(Phase phase, Board board);
    }
}