using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

// Ocelot Configuration
// note: using must be added manuelly using Ocelot.Cache.CacheManager;
builder.Services.AddOcelot().AddCacheManager(x => 
{
    x.WithDictionaryHandle();
});

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json",true, true);
// Logging Configuration https://docs.microsoft.com/en-us/aspnet/core/migration/50-to-60-samples?view=aspnetcore-6.0
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Ocelot Configuration
await app.UseOcelot();
app.MapGet("/", () => "Hello World!");

app.Run();
