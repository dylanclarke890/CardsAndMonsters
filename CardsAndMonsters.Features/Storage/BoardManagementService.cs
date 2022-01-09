using CardsAndMonsters.Models;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Storage
{
    public class BoardManagementService : IBoardManagementService
    {
        private readonly ILocalStorageService<Board> _localStorageService;
        private static readonly string StorageName = "boardStorage";


        public BoardManagementService(ILocalStorageService<Board> localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task Save(Board board)
        {
            await _localStorageService.SetItem(StorageName, board);
        }

        public async Task<Board> Load()
        {
            return await _localStorageService.GetItem(StorageName);
        }

        public async Task Delete()
        {
            await _localStorageService.DeleteItem(StorageName);
        }
    }
}
