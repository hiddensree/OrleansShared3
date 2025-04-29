using GrainInterfaces;
using GrainInterfaces.StreamManagement.Consumption;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace ProducerConsumerA.StreamManagement.Consumption;

public class StreamConsumption(ILogger<IStreamConsumption> logger) : IStreamConsumption
{
    public Task ConsumeStatisticsAsync(
        IStreamProvider streamProvider,
        string streamNamespace,
        string streamId
    )
    {
        var stream = streamProvider.GetStream<MachineData>(streamId, streamNamespace);
        logger.LogInformation(
            "Consuming stream {StreamId} in namespace {StreamNamespace}",
            streamId,
            streamNamespace
        );

        try
        {
            var subscriptionHandle = stream.SubscribeAsync(
                async (data, token) =>
                {
                    logger.LogInformation("Data Source: {Data}", data.Source);
                    logger.LogInformation("Data Retrieved: {Data}", data.Value); 
                    logger.LogInformation(
                        "StreamId: {StreamId}, StreamNamespace: {StreamNamespace}",
                        streamId,
                        streamNamespace
                    );
                    await Task.CompletedTask;
                }
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error consuming stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
            throw;
        }
        return Task.CompletedTask;
    }
}
