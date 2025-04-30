namespace LimPocHelperFramework.Models;

[GenerateSerializer]
[Serializable]
[Alias("LimPocHelperFramework.Models.IStreamData")]
public class IStreamData
{
    [Id(0)]
    string Source { get; set; } = string.Empty;

    [Id(1)]
    string Data { get; set; } = string.Empty;

}
