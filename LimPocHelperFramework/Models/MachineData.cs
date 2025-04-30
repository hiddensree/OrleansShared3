namespace LimPocHelperFramework.Models;

[GenerateSerializer]
[Serializable]
[Alias("LimPocHelperFramework.Models.MachineData")]
public class MachineData : IStreamData
{
    [Id(0)]
    public string MachineName { get; set; } = string.Empty;

    [Id(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Id(2)]
    public string Source { get; set; } = string.Empty;
}

