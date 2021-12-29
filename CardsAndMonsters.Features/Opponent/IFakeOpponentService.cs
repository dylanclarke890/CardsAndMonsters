using CardsAndMonsters.Models;
using System;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Opponent
{
    public interface IFakeOpponentService
    {
        Task FakeOpponentsTurn(Board board);
    }
}