namespace LimPocHelperFramework.Grains;

/// <summary>
/// This interface represents a grain with a GUID key that can be activated and produce data asynchronously.
/// </summary>
[Alias("LimPocHelperFramework.Grains.IGrainA")]
public interface IGrainA : IGrainWithGuidKey
{
    /// <summary>
    /// Activates the grain asynchronously.
    /// </summary>
    [Alias("ActivateAsyncA")]
    Task ActivateAsync();

    /// <summary>
    /// Starts producing data asynchronously.
    /// </summary>
    /// <returns></returns>
    [Alias("StartProducingAsyncA")]
    Task StartProducingAsync();
}
