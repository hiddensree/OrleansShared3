using Orleans;
using Orleans.Streams;

namespace GrainInterfaces;

//[ImplicitStreamSubscription("StreamB")]
[Alias("GrainInterfaces.IGrainA")]
public interface IGrainA : IGrainWithGuidKey
{
    [Alias("StartProducingAsyncA")]
    Task StartProducingAsync();

    [Alias("ActivateAsyncA")]
    Task ActivateAsync();
}
