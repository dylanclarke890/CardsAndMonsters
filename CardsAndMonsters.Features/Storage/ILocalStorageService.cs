using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Storage
{
    public interface ILocalStorageService
    {
        Task<object> GetItem(string key);
        Task SetItem(string key, object item);
    }
}