using GrainInterfaces.StreamManagement.Consumption;
using GrainInterfaces.StreamManagement.Production;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using ProducerConsumerA;
using ProducerConsumerA.StreamManagement.Consumption;
using ProducerConsumerA.StreamManagement.Production;

var host = new HostBuilder()
    .UseOrleans(silo =>
    {
        silo.UseLocalhostClustering()
            .AddStreaming()
            .AddMemoryStreams("OrleansStream")
            .UseLocalhostClustering(
                siloPort: 11111,
                gatewayPort: 30000,
                primarySiloEndpoint: new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 11111)
            )
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "shared-cluster";
                options.ServiceId = "shared-service";
            })
            .AddMemoryGrainStorage("PubSubStore")
            .ConfigureLogging(logging => logging.AddConsole())
            .AddStartupTask<ActivateGrainOnStartUpTask>()
            .ConfigureServices(services =>
            {
                services.AddTransient<IStreamConsumption, StreamConsumption>();
                services.AddTransient<IStreamProduction, StreamProduction>();
            });
    })
    .Build();

await host.StartAsync();
Console.WriteLine("Orleans host started.");
Console.WriteLine("Press Enter to terminate...");
Console.ReadLine();

//await client.CloseAsync();
await host.StopAsync();
