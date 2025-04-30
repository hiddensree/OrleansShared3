using LimPocHelperFramework.StartupTasks;
using Orleans;
using Orleans.Streams;

namespace LimPocHelperFramework.Grains;

//[ImplicitStreamSubscription("StreamB")]
[Alias("GrainInterfaces.IGrainA")]
public interface IGrainA : IGrainWithGuidKey
{

    [Alias("ActivateAsyncA")]
    Task ActivateAsync();
    
    [Alias("StartProducingAsyncA")]
    Task StartProducingAsync();

}
