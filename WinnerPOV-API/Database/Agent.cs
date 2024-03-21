

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WinnerPOV_API.Database;

public partial class Agent
{
    [JsonIgnore]
    public int AgentId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("thumbnailUrl")]
    public string? ThumbnailUrl { get; set; }

    [JsonPropertyName("portraitUrl")]
    public string? PortraitUrl { get; set; }

    [JsonIgnore]
    public virtual ICollection<PlayerMatch> PlayerMatches { get; } = new List<PlayerMatch>();
}
