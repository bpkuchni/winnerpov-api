
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WinnerPOV_API.Database;

public partial class Rank
{
    public int RankId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("iconSmallUrl")]
    public string? IconSmallUrl { get; set; }

    [JsonPropertyName("iconBigUrl")]
    public string? IconBigUrl { get; set; }

    [JsonIgnore]
    public virtual ICollection<Player> Players { get; } = new List<Player>();
}
