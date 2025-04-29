using GrainInterfaces;
using Orleans.Streams;

namespace GrainInterfaces.StreamManagement.Production;

public interface IStreamProduction
{
    Task ProduceMachineItemsForControllerAsync(
        IStreamProvider streamProvider,
        string streamNamespace,
        string streamId,
        MachineData data
    );
}
