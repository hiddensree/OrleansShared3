using LimPocHelperFramework.Grains;
using LimPocHelperFramework.Models;
using LimPocHelperFramework.StreamManagement.Consumption;
using LimPocHelperFramework.StreamManagement.Production;
using Microsoft.Extensions.Logging;

namespace ProducerConsumerA;

public class GrainA(
    IStreamProduction<MachineData> streamProduction,
    IStreamConsumption<StreamData> streamConsumption,
    ILogger<IGrainA> logger
) : Grain, IGrainA
{
    private const string StreamProviderName = "OrleansStream";
    private const string StreamControllerNamespace = "controller";
    private const string StreamControllerId = "controller";
    private const string StreamNamespace = "field";
    private const string StreamId = "field";
    private readonly ILogger<IGrainA> _logger = logger;
    private readonly IStreamProduction<MachineData> _streamProduction = streamProduction;
    private readonly IStreamConsumption<StreamData> _streamConsumption = streamConsumption;
    private IDisposable? _timerHandle;

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        return base.OnActivateAsync(cancellationToken);
    }

    public async Task ActivateAsync()
    {
        Console.WriteLine($"GrainA activated: {this.GetPrimaryKey()}");
        _timerHandle ??= this.RegisterGrainTimer(
            async _ => await StartProducingAsync(),
            this,
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(10)
        );

        _logger.LogInformation("GrainA activated: {GrainId}", this.GetPrimaryKey());
        _logger.LogInformation("Starting data consumption for stream {StreamId}", StreamId);
        await ControllerDataConsumptionAsync(
            StreamControllerId,
            StreamControllerNamespace,
            StreamProviderName
        );
    }

    public async Task StartProducingAsync()
    {
        try
        {
            var data = new MachineData
            {
                MachineName = "Machine1",
                Timestamp = DateTime.UtcNow,
                Source = "ProducerConsumerA",
            };
            var streamProvider = this.GetStreamProvider(StreamProviderName);
            await _streamProduction.ProduceAsync(streamProvider, StreamNamespace, StreamId, data);
            _logger.LogInformation("Produced data: {Data}", data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error producing data: {Message}", ex.Message);
        }
    }

    public async Task ControllerDataConsumptionAsync(
        string streamId,
        string streamNamespace,
        string streamProviderName
    )
    {
        try
        {
            var streamProvider = this.GetStreamProvider(streamProviderName);
            await _streamConsumption.ConsumeAsync(
                streamProvider,
                streamNamespace,
                streamId,
                async (data, token) =>
                {
                    _logger.LogInformation("Data Source: {Data}", data.Source);
                    _logger.LogInformation("Data Retrieved: {Data}", data.Value);
                    _logger.LogInformation(
                        "StreamId: {StreamId}, StreamNamespace: {StreamNamespace}",
                        streamId,
                        streamNamespace
                    );
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error consuming data: {Message}", ex.Message);
        }
    }

    public override Task OnDeactivateAsync(
        DeactivationReason reason,
        CancellationToken cancellationToken
    )
    {
        _timerHandle?.Dispose();
        _timerHandle = null;
        return base.OnDeactivateAsync(reason, cancellationToken);
    }
}
