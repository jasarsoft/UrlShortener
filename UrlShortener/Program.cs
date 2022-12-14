using Azure.Identity;
using Microsoft.AspNetCore.Http.Extensions;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;
using UrlShortener;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddMemoryGrainStorage("urls");
    //siloBuilder.AddAzureBlobGrainStorage("urls",
    //    // Recommended: Connect to Blob Storage using DefaultAzureCredential
    //    options =>
    //    {
    //        options.ConfigureBlobServiceClient(new Uri("https://<your-account-name>.blob.core.windows.net"),
    //            new DefaultAzureCredential());
    //    });
    //// Connect to Blob Storage using Connection strings
    //// options => options.ConfigureBlobServiceClient(connectionString));
});

var app = builder.Build();

var grainFactory = app.Services.GetRequiredService<IGrainFactory>();

app.MapGet("/", () => "Hello World!");
app.MapGet("/shorten/{*path}",
    async (IGrainFactory grains, HttpRequest request, string path) =>
    {
        var shortenedRouteSegment = Guid.NewGuid().GetHashCode().ToString("X");
        var shortenerGrain = grains.GetGrain<IUrlShortenerGrain>(shortenedRouteSegment);
        await shortenerGrain.SetUrl(shortenedRouteSegment, path);
        var resultBuilder = new UriBuilder(request.GetEncodedUrl())
        {
            Path = $"/go/{shortenedRouteSegment}"
        };

        return Results.Ok(resultBuilder.Uri);
    });
app.MapGet("/go/{shortenedRouteSegment}",
    async (IGrainFactory grains, string shortenedRouteSegment) =>
    {
        var shortenerGrain = grains.GetGrain<IUrlShortenerGrain>(shortenedRouteSegment);
        var url = await shortenerGrain.GetUrl();

        return Results.Redirect(url);
    });

app.Run();
