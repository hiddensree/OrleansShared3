namespace LimPocHelperFramework.Grains;

/// <summary>
/// This interface represents a grain with a GUID key that can be activated and produce data asynchronously.
/// </summary>
[Alias("LimPocHelperFramework.Grains.IGrainB")]
public interface IGrainB : IGrainWithGuidKey
{
    /// <summary>
    /// Starts producing data asynchronously.
    /// </summary>
    /// <returns></returns>
    [Alias("StartProducingAsync")]
    Task StartProducingAsync();

    /// <summary>
    /// Activates the grain asynchronously.
    /// </summary>
    /// <returns></returns>
    [Alias("ActivateAsync")]
    Task ActivateAsync();

    /// <summary>
    /// Stops the grain asynchronously.
    /// </summary>
    /// <returns></returns>
    [Alias("StopGrainBAsync")]
    Task StopGrainBAsync();
}
