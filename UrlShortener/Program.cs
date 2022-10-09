using Microsoft.AspNetCore.Http.Extensions;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
