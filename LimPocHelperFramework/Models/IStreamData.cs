namespace LimPocHelperFramework.Models;


/// <summary>
/// Represents the data structure for streaming data with a source and associated data.
/// </summary>
[GenerateSerializer]
[Serializable]
[Alias("LimPocHelperFramework.Models.IStreamData")]
public class IStreamData
{
    [Id(0)]
    public string Source { get; set; } = string.Empty;

    [Id(1)]
    public string Data { get; set; } = string.Empty;

}
