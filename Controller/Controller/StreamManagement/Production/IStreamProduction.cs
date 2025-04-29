using GrainInterfaces;
using Orleans.Streams;

namespace Controller.StreamManagement.Production;

public interface IStreamProduction
{
    Task ProduceStatisticsForFieldConnectorAsync(
        IStreamProvider streamProvider,
        string streamNamespace,
        string streamId,
        MachineData data
    );
}
