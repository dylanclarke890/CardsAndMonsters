using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Storage
{
    public interface ILocalStorageService<T> where T : class
    {
        Task<T> GetItem(string key);
        Task SetItem(string key, T item);
    }
}