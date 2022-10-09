using Orleans;

namespace UrlShortener
{
    public class UrlShortenerGrain : Grain, IUrlShortenerGrain
    {
        private KeyValuePair<string, string> _cache;

        public Task SetUrl(string shortenedRouteSegment, string fullUrl)
        {
            _cache = new KeyValuePair<string, string>(shortenedRouteSegment, fullUrl);
            return Task.CompletedTask;
        }

        public Task<string?> GetUrl()
        {
            return Task.FromResult(_cache.Value);
        }
    }
}
