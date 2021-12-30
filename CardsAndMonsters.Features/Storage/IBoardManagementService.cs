using CardsAndMonsters.Models;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Storage
{
    public interface IBoardManagementService
    {
        Task<Board> Load();
        Task Save(Board board);
        Task Delete();
    }
}