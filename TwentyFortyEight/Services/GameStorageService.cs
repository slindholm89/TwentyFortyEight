using Microsoft.JSInterop;
using TwentyFortyEight.Models;

namespace TwentyFortyEight.Services {
    public sealed class GameStorageService : IAsyncDisposable {
        private readonly Lazy<Task<IJSObjectReference>> _module;

        public GameStorageService(IJSRuntime js) {
            _module = new(() =>
                js.InvokeAsync<IJSObjectReference>(
                    "import", "./js/gameStorage.js").AsTask());
        }

        public async Task<GameStorageData?> LoadAsync() {
            var module = await _module.Value;

            return await module.InvokeAsync<GameStorageData?>("load");
        }

        public async Task SaveAsync(GameStorageData data) {
            var module = await _module.Value;

            await module.InvokeVoidAsync("save", data);
        }

        public async ValueTask DisposeAsync() {
            if (!_module.IsValueCreated)
                return;

            var module = await _module.Value;
            await module.DisposeAsync();
        }
    }
}
