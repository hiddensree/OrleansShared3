using System;
using GrainInterfaces;
using Microsoft.Extensions.Logging;

namespace Controller;

public class ActiveGrainOnStartUpTask(
    IGrainFactory _grainFactory,
    ILogger<ActiveGrainOnStartUpTask> logger
) : IStartupTask
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        try
        {
            var grain2 = _grainFactory.GetGrain<IGrainB>(Guid.NewGuid());
            await grain2.ActivateAsync();
        }
        catch (Exception)
        {
            logger.LogError("Error activating grains");
            throw;
        }
    }
}
