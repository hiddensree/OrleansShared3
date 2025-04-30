namespace LimPocHelperFramework.Grains;

//[ImplicitStreamSubscription("StreamB")]
[Alias("GrainInterfaces.IGrainB")]
public interface IGrainB : IGrainWithGuidKey
{
    [Alias("StartProducingAsync")]
    Task StartProducingAsync();

    [Alias("ActivateAsync")]
    Task ActivateAsync();

    [Alias("StopGrainBAsync")]
    Task StopGrainBAsync();
}
