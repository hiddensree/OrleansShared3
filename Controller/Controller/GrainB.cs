using Controller.StreamManagement.Consumption;
using Controller.StreamManagement.Production;
using GrainInterfaces;
using Microsoft.Extensions.Logging;

namespace Controller;

public class GrainB(
    IStreamProduction streamProduction,
    IStreamConsumption streamConsumption,
    ILogger<IGrainB> logger
) : Grain, IGrainB
{
    private const string StreamProviderName = "OrleansStream";
    private const string StreamControllerNamespace = "controller";
    private const string StreamControllerId = "controller";
    private const string StreamNamespace = "field";
    private const string StreamId = "field";
    private readonly ILogger<IGrainB> _logger = logger;
    private readonly IStreamProduction _streamProduction = streamProduction;
    private readonly IStreamConsumption _streamConsumption = streamConsumption;

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("GrainB OnActivateAsync: {GrainId}", this.GetPrimaryKey());
        return base.OnActivateAsync(cancellationToken);
    }

    public async Task ActivateAsync()
    {
        _logger.LogInformation(
            "GrainB ActivateAsync: Starting for {GrainId}",
            this.GetPrimaryKey()
        );
        await FieldConnectorDataConsumptionAsync(StreamId, StreamNamespace, StreamProviderName);
        _logger.LogInformation("GrainB activated: {GrainId}", this.GetPrimaryKey());
    }

    public async Task StartProducingAsync()
    {
        var data = new StreamData
        {
            Source = "Controller Stream Data",
            Value = Random.Shared.Next(1, 1000),
        };
        var streamProvider = this.GetStreamProvider(StreamProviderName);
        await _streamProduction.ProduceStatisticsForFieldConnectorAsync(
            streamProvider,
            StreamControllerNamespace,
            StreamControllerId,
            data
        );
        _logger.LogInformation(
            "GrainB Produced data for stream {StreamId} in namespace {StreamNamespace}",
            StreamControllerId,
            StreamControllerNamespace
        );
    }

    public async Task FieldConnectorDataConsumptionAsync(
        string streamId,
        string streamNamespace,
        string streamProviderName
    )
    {
        try
        {
            _logger.LogInformation(
                "GrainB Setting up consumption for stream {StreamId} in namespace {StreamNamespace} with provider {Provider}",
                streamId,
                streamNamespace,
                streamProviderName
            );
            var streamProvider = this.GetStreamProvider(streamProviderName);
            await _streamConsumption.ConsumeProduction(streamProvider, streamNamespace, streamId);
            _logger.LogInformation(
                "GrainB Successfully set up consumption for stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
            // Produce data after subscription to test the loop
            //await StartProducingAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "GrainB Error setting up consumption for stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
            throw;
        }
    }

    public Task StopGrainBAsync()
    {
        _logger.LogInformation(
            "GrainB StopGrainBAsync: Stopping for {GrainId}",
            this.GetPrimaryKey()
        );
        DeactivateOnIdle();
        return Task.CompletedTask;
    }

    public override Task OnDeactivateAsync(
        DeactivationReason reason,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(
            "GrainB Deactivating: {GrainId}, Reason: {Reason}",
            this.GetPrimaryKey(),
            reason.Description
        );
        return base.OnDeactivateAsync(reason, cancellationToken);
    }
}
