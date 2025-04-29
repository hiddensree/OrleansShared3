using Orleans;
using Orleans.Streams;

namespace GrainInterfaces;

//[ImplicitStreamSubscription("StreamB")]
[Alias("GrainInterfaces.IGrainB")]
public interface IGrainB : IGrainWithGuidKey
{
    [Alias("StartProducingAsync")]
    Task StartProducingAsync();

    [Alias("ActivateAsync")]
    Task ActivateAsync();
}
