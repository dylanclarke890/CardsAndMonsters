using CardsAndMonsters.Models.Base;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Storage
{
    public class LocalStorageService<T> : ILocalStorageService<T> where T : BaseModel
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly JsonSerializerSettings _serializerSettings = new() 
        { 
            TypeNameHandling = TypeNameHandling.Auto,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public LocalStorageService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public async Task SetItem(string key, T item)
        {
            var itemAsString = JsonConvert.SerializeObject(item, _serializerSettings);
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(itemAsString))
            {
                return;
            }

            await _jSRuntime.InvokeVoidAsync("setItem", key, itemAsString);
        }

        public async Task<T> GetItem(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            var item = await _jSRuntime.InvokeAsync<string>("getItem", key);

            return item != null ? JsonConvert.DeserializeObject<T>(item, _serializerSettings) : null;
        }

        public async Task DeleteItem(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            await _jSRuntime.InvokeVoidAsync("deleteItem", key);
        }
    }
}
