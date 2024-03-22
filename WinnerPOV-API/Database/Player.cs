
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WinnerPOV_API.Database;

public partial class Player
{
    [JsonIgnore]
    public int PlayerId { get; set; }

    [JsonIgnore]
    public int? RankId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } 

    [JsonPropertyName("tag")]
    public string Tag { get; set; }

    [JsonPropertyName("level")]
    public int? Level { get; set; }

    [JsonPropertyName("thumbnailUrl")]
    public string? ThumbnailUrl { get; set; }

    [JsonPropertyName("portraitUrl")]
    public string? PortraitUrl { get; set; }

    [JsonPropertyName("landscapeUrl")]
    public string? LandscapeUrl { get; set; }

    [JsonPropertyName("mmr")]
    public int? MatchmakingRating { get; set; }

    [JsonPropertyName("isEligible")]
    public sbyte? CurrentSeasonEligible { get; set; }

    [JsonPropertyName("score")]
    public int? CurrentSeasonScore { get; set; }

    [JsonIgnore]
    public virtual ICollection<HenrikPlayerMapping> HenrikPlayerMappings { get; } = new List<HenrikPlayerMapping>();

    [JsonIgnore]
    public virtual ICollection<PlayerMatch> PlayerMatches { get; } = new List<PlayerMatch>();

    [JsonPropertyName("rank")]
    public virtual Rank? Rank { get; set; }
}
