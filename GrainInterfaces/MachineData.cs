using System;
using Orleans;

namespace GrainInterfaces;

[GenerateSerializer]
[Serializable]
public class MachineData
{
    [Id(0)]
    public string Source { get; set; } = string.Empty;

    [Id(1)]
    public int Value { get; set; } = 0;

    [Id(2)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Id(3)]
    public string MachineName { get; set; } = string.Empty;
}
