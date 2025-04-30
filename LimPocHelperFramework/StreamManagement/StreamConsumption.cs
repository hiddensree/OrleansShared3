using LimPocHelperFramework.Models;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace LimPocHelperFramework.StreamManagement.Consumption;

/// <summary>
/// Defines a contract for consuming streams of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of data in the stream, which must implement <see cref="IStreamData"/>.</typeparam>
public interface IStreamConsumption<T>
    where T : IStreamData
{
    /// <summary>
    /// Consumes a stream by subscribing to it and processing incoming data.
    /// </summary>
    /// <param name="streamProvider">The stream provider to use for retrieving the stream.</param>
    /// <param name="streamNamespace">The namespace of the stream.</param>
    /// <param name="streamId">The identifier of the stream.</param>
    /// <param name="onDataReceived">The callback to invoke when data is received on the stream.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ConsumeAsync(
        IStreamProvider streamProvider,
        string streamNamespace,
        string streamId,
        Func<T, StreamSequenceToken, Task> onDataReceived
    );
}

/// <summary>
/// Provides functionality to consume streams of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of data in the stream, which must implement <see cref="IStreamData"/>.</typeparam>
public class StreamConsumption<T>(ILogger<IStreamConsumption<T>> logger) : IStreamConsumption<T>
    where T : IStreamData
{
    private readonly ILogger<IStreamConsumption<T>> _logger = logger;

    /// <summary>
    /// Consumes a stream by subscribing to it and processing incoming data.
    /// </summary>
    /// <param name="streamProvider">The stream provider to use for retrieving the stream.</param>
    /// <param name="streamNamespace">The namespace of the stream.</param>
    /// <param name="streamId">The identifier of the stream.</param>
    /// <param name="onDataReceived">The callback to invoke when data is received on the stream.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ConsumeAsync(
        IStreamProvider streamProvider,
        string streamNamespace,
        string streamId,
        Func<T, StreamSequenceToken, Task> onDataReceived
    )
    {
        try
        {
            _logger.LogInformation(
                "Creating stream for {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
            var stream = streamProvider.GetStream<T>(streamId, streamNamespace);
            _logger.LogInformation(
                "Subscribing to stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );

            var subscriptionHandle = await stream.SubscribeAsync(
                async (data, token) =>
                {
                    _logger.LogInformation(
                        "Received data on stream {StreamId} in namespace {StreamNamespace}",
                        streamId,
                        streamNamespace
                    );
                    await onDataReceived(data, token);
                },
                exception =>
                {
                    _logger.LogError(
                        exception,
                        "Error in subscription for stream {StreamId} in namespace {StreamNamespace}",
                        streamId,
                        streamNamespace
                    );
                    return Task.CompletedTask;
                }
            );

            _logger.LogInformation(
                "Subscription completed for stream {StreamId} in namespace {StreamNamespace}, Handle: {Handle}",
                streamId,
                streamNamespace,
                subscriptionHandle?.GetHashCode() ?? 0
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to subscribe to stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
            throw;
        }
    }
}
