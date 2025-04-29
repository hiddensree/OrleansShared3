using System;
using Orleans.Streams;

namespace Controller.StreamManagement.Consumption;

public interface IStreamConsumption
{
    Task ConsumeProduction(IStreamProvider streamProvider, string streamNamespace, string streamId);
}
