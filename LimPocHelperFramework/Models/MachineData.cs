namespace LimPocHelperFramework.Models;

/// <summary>
/// Represents the data structure for streaming data with a source and associated data.
/// </summary>
[GenerateSerializer]
[Serializable]
[Alias("LimPocHelperFramework.Models.MachineData")]
public class MachineData : IStreamData
{
    /// <summary>
    /// The name of the machine where the data is generated.
    /// </summary>
    [Id(0)]
    public string MachineName { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp when the data was generated.
    /// </summary>
    [Id(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The data associated with the stream.
    /// </summary>
    [Id(2)]
    public string Source { get; set; } = string.Empty;
}

