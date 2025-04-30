using Microsoft.Extensions.Logging;

namespace LimPocHelperFramework.StartupTasks;

/// <summary>
/// This class represents a generic startup task for activating grains of type TGrain.
/// </summary>
/// <typeparam name="TGrain"></typeparam>
/// <param name="grainFactory"></param>
/// <param name="logger"></param>
public class GenericStartupTask<TGrain>(
    IGrainFactory grainFactory,
    ILogger<GenericStartupTask<TGrain>> logger
) : IStartupTask
    where TGrain : IBasicGrain
{
    private readonly IGrainFactory _grainFactory = grainFactory;
    private readonly ILogger<GenericStartupTask<TGrain>> _logger = logger;

    /// <summary>
    /// Executes the startup task for the specified grain type.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

/// <summary>
/// This interface represents a grain with a GUID key that can be activated and produce data asynchronously.
/// </summary>
public interface IBasicGrain : IGrainWithGuidKey
{
    [Alias("ActivateAsync")]
    Task ActivateAsync();
}
