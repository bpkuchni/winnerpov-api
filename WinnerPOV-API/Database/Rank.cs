using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class Rank
{
    public int RankId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("iconSmallUrl")]
    public string? IconSmallUrl { get; set; }

    [JsonProperty("iconBigUrl")]
    public string? IconBigUrl { get; set; }

    public virtual ICollection<Player> Players { get; } = new List<Player>();
}
