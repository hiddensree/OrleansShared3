using Orleans.Streams;

namespace GrainInterfaces.StreamManagement.Consumption;

public interface IStreamConsumption
{
    Task ConsumeStatisticsAsync(
        IStreamProvider streamProvider,
        string streamNamespace,
        string streamId
    );
}
