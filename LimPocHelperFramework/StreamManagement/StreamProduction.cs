using LimPocHelperFramework.Models;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace LimPocHelperFramework.StreamManagement.Production;

/// <summary>
/// Defines a contract for producing data to an Orleans stream.
/// </summary>
/// <typeparam name="T">The type of data to be produced, which must implement <see cref="IStreamData"/>.</typeparam>
public interface IStreamProduction<T>
    where T : IStreamData
{
    /// <summary>
    /// Produces data to an Orleans stream.
    /// </summary>
    /// <param name="streamProvider">The stream provider to use for producing data.</param>
    /// <param name="streamNamespace">The namespace of the stream.</param>
    /// <param name="streamId">The identifier of the stream.</param>
    /// <param name="data">The data to be produced to the stream.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ProduceAsync(
        IStreamProvider streamProvider,
        string streamNamespace,
        string streamId,
        T data
    );
}

/// <summary>
/// Implements the <see cref="IStreamProduction{T}"/> interface to produce data to an Orleans stream.
/// </summary>
/// <typeparam name="T">The type of data to be produced, which must implement <see cref="IStreamData"/>.</typeparam>
public class StreamProduction<T>(ILogger<IStreamProduction<T>> logger) : IStreamProduction<T>
    where T : IStreamData
{
    private readonly ILogger<IStreamProduction<T>> _logger = logger;

    /// <inheritdoc/>
    public async Task ProduceAsync(
        IStreamProvider streamProvider,
        string streamNamespace,
        string streamId,
        T data
    )
    {
        try
        {
            var stream = streamProvider.GetStream<T>(streamId, streamNamespace);
            _logger.LogInformation(
                "Producing data for stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
            await stream.OnNextAsync(data);
            _logger.LogInformation(
                "Produced data for stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error producing data for stream {StreamId} in namespace {StreamNamespace}",
                streamId,
                streamNamespace
            );
            throw;
        }
    }
}
