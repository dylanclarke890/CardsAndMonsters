using CardsAndMonsters.Models;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Turn
{
    public interface ITurnService
    {
        Task EndTurn(Board board);
        Task ResumeTurn(Board board);
        Task StartTurn(Duelist player, bool drawCard, Board board);
    }
}