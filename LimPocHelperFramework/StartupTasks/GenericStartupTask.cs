using Microsoft.Extensions.Logging;

namespace LimPocHelperFramework.StartupTasks;

public class GenericStartupTask<TGrain>(
    IGrainFactory grainFactory,
    ILogger<GenericStartupTask<TGrain>> logger
) : IStartupTask
    where TGrain : IBasicGrain
{
    private readonly IGrainFactory _grainFactory = grainFactory;
    private readonly ILogger<GenericStartupTask<TGrain>> _logger = logger;

    public async Task Execute(CancellationToken cancellationToken)
    {
        try
        {
            var grain = _grainFactory.GetGrain<TGrain>(Guid.NewGuid());
            await grain.ActivateAsync();
            _logger.LogInformation("Activated grain of type {GrainType}", typeof(TGrain).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating grain of type {GrainType}", typeof(TGrain).Name);
            throw;
        }
    }
}

public interface IBasicGrain : IGrainWithGuidKey
{
    [Alias("ActivateAsync")]
    Task ActivateAsync();
}
