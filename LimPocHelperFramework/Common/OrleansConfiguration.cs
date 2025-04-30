using LimPocHelperFramework.Models;
using LimPocHelperFramework.StreamManagement.Consumption;
using LimPocHelperFramework.StreamManagement.Production;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;

namespace LimPocHelperFramework.Common;

public static class OrleansConfiguration
{
    /// <summary>
    /// Configures the Orleans host with the specified silo, gateway, and dashboard ports.
    /// </summary>
    /// <param name="builder">The host builder to configure.</param>
    /// <param name="siloPort">The port for the Orleans silo.</param>
    /// <param name="gatewayPort">The port for the Orleans gateway.</param>
    /// <param name="dashboardPort">The port for the Orleans dashboard (default is 8088).</param>
    /// <returns>The configured host builder.</returns>
    public static IHostBuilder ConfigureOrleansHost(
        this IHostBuilder builder,
        int siloPort,
        int gatewayPort,
        int dashboardPort = 8088
    )
    {
        return builder
            .UseOrleans(silo =>
            {
                silo.UseLocalhostClustering()
                    .AddStreaming()
                    .AddMemoryStreams("OrleansStream")
                    .UseLocalhostClustering(
                        siloPort: siloPort,
                        gatewayPort: gatewayPort,
                        primarySiloEndpoint: new System.Net.IPEndPoint(
                            System.Net.IPAddress.Loopback,
                            11111
                        )
                    )
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "shared-cluster";
                        options.ServiceId = "shared-service";
                    })
                    .AddMemoryGrainStorage("PubSubStore")
                    .ConfigureLogging(logging => logging.AddConsole())
                    .ConfigureServices(services =>
                    {
                        // Register services for both MachineData and StreamData
                        services.AddTransient<
                            IStreamConsumption<MachineData>,
                            StreamConsumption<MachineData>
                        >();
                        services.AddTransient<
                            IStreamProduction<MachineData>,
                            StreamProduction<MachineData>
                        >();
                        services.AddTransient<
                            IStreamConsumption<StreamData>,
                            StreamConsumption<StreamData>
                        >();
                        services.AddTransient<
                            IStreamProduction<StreamData>,
                            StreamProduction<StreamData>
                        >();
                    })
                    .UseDashboard(options => options.Port = dashboardPort);
            })
            .ConfigureLogging(logging =>
                logging.AddConsole().SetMinimumLevel(LogLevel.Information)
            );
    }
}
