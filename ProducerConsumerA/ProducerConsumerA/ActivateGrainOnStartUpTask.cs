using System;
using GrainInterfaces;
using Microsoft.Extensions.Logging;

namespace ProducerConsumerA;

public class ActivateGrainOnStartUpTask(
    IGrainFactory _grainFactory,
    ILogger<ActivateGrainOnStartUpTask> logger
) : IStartupTask
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        try
        {
            var grain2 = _grainFactory.GetGrain<IGrainA>(Guid.NewGuid());
            await grain2.ActivateAsync();
        }
        catch (Exception)
        {
            logger.LogError("Error activating grains");
            throw;
        }
    }
}
