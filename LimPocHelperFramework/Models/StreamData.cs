namespace LimPocHelperFramework.Models;

[GenerateSerializer]
[Serializable]
[Alias("LimPocHelperFramework.Models.StreamData")]
public class StreamData : IStreamData
{
    [Id(0)]
    public string Source { get; set; } = string.Empty;

    [Id(1)]
    public int Value { get; set; } = 0;
}
