using Orleans;
using Orleans.Runtime;

namespace UrlShortener
{
    public class UrlShortenerGrain : Grain, IUrlShortenerGrain
    {
        private IPersistentState<KeyValuePair<string, string>> _cache;

        public UrlShortenerGrain(
            [PersistentState(
                stateName: "url",
                storageName: "urls")]
            IPersistentState<KeyValuePair<string, string>> state)
        {
            _cache = state;
        }

        public async Task SetUrl(string shortenedRouteSegment, string fullUrl)
        {
            _cache.State = new KeyValuePair<string, string>(shortenedRouteSegment, fullUrl);
            await _cache.WriteStateAsync();
        }

        public Task<string> GetUrl()
        {
            return Task.FromResult(_cache.State.Value);
        }
    }
}
