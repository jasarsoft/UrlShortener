using Orleans;

namespace UrlShortener
{
    public interface IUrlShortenerGrain : IGrainWithStringKey
    {
        Task SetUrl(string shortenedRouteSegment, string fullUrl);
        Task<string> GetUrl();
    }
}
