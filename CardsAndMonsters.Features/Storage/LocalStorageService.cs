using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CardsAndMonsters.Features.Storage
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jSRuntime;

        public LocalStorageService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public async Task SetItem(string key, object item)
        {
            var itemAsString = JsonConvert.SerializeObject(item);
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(itemAsString))
            {
                return;
            }

            await _jSRuntime.InvokeVoidAsync("setItem", key, itemAsString);
        }

        public async Task<object> GetItem(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            return await _jSRuntime.InvokeAsync<object>("getItem", key);
        }
    }
}
