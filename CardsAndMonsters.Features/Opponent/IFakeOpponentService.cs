using CardsAndMonsters.Models;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Opponent
{
    public interface IFakeOpponentService
    {
        Task FakeBattlePhase(Board board);
        Task FakeEndPhase(Board board);
        Task FakeMainPhase(Board board);
        Task ResumePhase(Board board);
    }
}