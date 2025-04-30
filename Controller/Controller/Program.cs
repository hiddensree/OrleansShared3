using LimPocHelperFramework.Common;
using LimPocHelperFramework.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureOrleansHost(siloPort: 11112, gatewayPort: 30001, dashboardPort: 8098)
    .Build();

await host.StartAsync();

// Directly activate GrainB
var grainFactory = host.Services.GetRequiredService<IGrainFactory>();
var grainB = grainFactory.GetGrain<IGrainB>(Guid.NewGuid());
await grainB.ActivateAsync(); // Call ActivateAsync defined in IGrainBase

Console.WriteLine("Orleans Controller host started and GrainB activated.");
Console.WriteLine("Press Enter to terminate...");
Console.ReadLine();

await host.StopAsync();
