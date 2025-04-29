using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace Controller.StreamManagement.Consumption;

public class StreamConsumption(ILogger<IStreamConsumption> logger, IGrainFactory grainFactory)
    : IStreamConsumption
{
    public async Task ConsumeProduction(
        IStreamProvider streamProvider,
        string streamNamespace,
        string streamId
    )
    {
        try
        {
            logger.LogInformation(
                "StreamConsumption: Creating stream for {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
            var stream = streamProvider.GetStream<MachineData>(streamId, streamNamespace);
            logger.LogInformation(
                "StreamConsumption: Subscribing to stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );

            var subscriptionHandle = await stream.SubscribeAsync(
                async (data, token) =>
                {
                    logger.LogInformation(
                        "StreamConsumption: Received data on stream {StreamId} in namespace {StreamNamespace}",
                        streamId,
                        streamNamespace
                    );
                    logger.LogInformation("Machine Name: {MachineName}", data.MachineName);
                    logger.LogInformation("Data Retrieved: {Value}", data.Value);
                    logger.LogInformation("Data Source: {Source}", data.Source);
                    logger.LogInformation("Data Timestamp: {Timestamp}", data.Timestamp);

                    // Get a reference to GrainB
                    var grainB = grainFactory.GetGrain<IGrainB>(Guid.NewGuid()); // maybe pass the same id from the grain.
                    logger.LogInformation(
                        "GrainB reference obtained: {GrainId}",
                        grainB.GetPrimaryKey()
                    );

                    // Call a method on GrainB
                    await grainB.StartProducingAsync();
                    await grainB.StopGrainBAsync();

                    await Task.CompletedTask;
                },
                (exception) =>
                {
                    logger.LogError(
                        exception,
                        "StreamConsumption: Error in subscription for stream {StreamId} in namespace {StreamNamespace}",
                        streamId,
                        streamNamespace
                    );
                    return Task.CompletedTask;
                }
            );

            logger.LogInformation(
                "StreamConsumption: Subscription completed for stream {StreamId} in namespace {StreamNamespace}, Handle: {Handle}",
                streamId,
                streamNamespace,
                subscriptionHandle?.GetHashCode() ?? 0
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "StreamConsumption: Failed to subscribe to stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
            throw;
        }
    }
}
