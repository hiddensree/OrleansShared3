using LimPocHelperFramework.Common;
using LimPocHelperFramework.Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder().ConfigureOrleansHost(siloPort: 11111, gatewayPort: 30000).Build();

await host.StartAsync();

// Directly activate GrainA
var grainFactory = host.Services.GetRequiredService<IGrainFactory>();
var grainA = grainFactory.GetGrain<IGrainA>(Guid.NewGuid());
await grainA.ActivateAsync(); // Call ActivateAsync defined in IGrainBase
await grainA.StartProducingAsync(); // Start producing data

Console.WriteLine("Orleans host started and GrainA activated.");
Console.WriteLine("Press Enter to terminate...");
Console.ReadLine();

await host.StopAsync();
