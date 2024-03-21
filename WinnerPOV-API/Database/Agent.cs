using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class Agent
{
    public int AgentId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("thumbnailUrl")]
    public string? ThumbnailUrl { get; set; }

    [JsonProperty("portraitUrl")]
    public string? PortraitUrl { get; set; }

    public virtual ICollection<PlayerMatch> PlayerMatches { get; } = new List<PlayerMatch>();
}
