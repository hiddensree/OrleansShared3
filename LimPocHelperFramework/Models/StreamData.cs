namespace LimPocHelperFramework.Models;

/// <summary>
/// Represents the data structure for streaming data with a source and associated data.
/// </summary>
[GenerateSerializer]
[Serializable]
[Alias("LimPocHelperFramework.Models.StreamData")]
public class StreamData : IStreamData
{
    /// <summary>
    /// The source of the data stream.
    /// </summary>
    [Id(0)]
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// The data associated with the stream.
    /// </summary>
    [Id(1)]
    public int Value { get; set; } = 0;
}
