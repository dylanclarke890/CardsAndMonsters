using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Storage
{
    public interface ILocalStorageService<T> where T : class
    {
        Task DeleteItem(string key);
        Task<T> GetItem(string key);
        Task SetItem(string key, T item);
    }
}