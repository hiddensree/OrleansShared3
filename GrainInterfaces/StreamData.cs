using System;
using Orleans;

namespace GrainInterfaces;

[GenerateSerializer]
[Serializable]
public class StreamData
{
    [Id(0)]
    public string Source { get; set; } = string.Empty;

    [Id(1)]
    public int Value { get; set; } = 0;
}
