using Controller;
using Controller.StreamManagement.Consumption;
using Controller.StreamManagement.Production;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;

var host = new HostBuilder()
    .UseOrleans(silo =>
    {
        silo.UseLocalhostClustering()
            .AddStreaming()
            .AddMemoryStreams("OrleansStream")
            .UseLocalhostClustering(
                siloPort: 11112,
                gatewayPort: 30001,
                primarySiloEndpoint: new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 11111)
            )
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "shared-cluster";
                options.ServiceId = "shared-service";
            })
            .AddMemoryGrainStorage("PubSubStore")
            .ConfigureLogging(logging => logging.AddConsole())
            .AddStartupTask<ActiveGrainOnStartUpTask>()
            .ConfigureServices(services =>
            {
                services.AddTransient<IStreamConsumption, StreamConsumption>();
                services.AddTransient<IStreamProduction, StreamProduction>();
            })
            .UseDashboard(options => options.Port = 8088);
    })
    .ConfigureLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Information))
    .Build();

await host.StartAsync();
Console.WriteLine("Orleans Controller host started.");
Console.WriteLine("Press Enter to terminate...");
Console.ReadLine();

//await client.CloseAsync();
await host.StopAsync();
