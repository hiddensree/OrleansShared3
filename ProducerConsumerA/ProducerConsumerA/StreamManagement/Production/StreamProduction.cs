using GrainInterfaces;
using GrainInterfaces.StreamManagement.Production;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace ProducerConsumerA.StreamManagement.Production;

public class StreamProduction(ILogger<IStreamProduction> logger) : IStreamProduction
{
    public async Task ProduceMachineItemsForControllerAsync(
        IStreamProvider streamProvider,
        string streamNamespace,
        string streamId,
        MachineData data
    )
    {
        try
        {
            var stream = streamProvider.GetStream<MachineData>(streamId, streamNamespace);
            logger.LogInformation(
                "Producing data for stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );

            await stream.OnNextAsync(data);
            logger.LogInformation(
                "Produced data for stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
            logger.LogInformation("Produced data: {Data}", data);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error producing data for stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
        }
    }
}
