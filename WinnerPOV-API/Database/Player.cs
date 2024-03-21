using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class Player
{
    public int PlayerId { get; set; }

    public int? RankId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("tag")]
    public string Tag { get; set; } = null!;

    [JsonProperty("level")]
    public int? Level { get; set; }

    [JsonProperty("thumbnailUrl")]
    public string? ThumbnailUrl { get; set; }

    [JsonProperty("portraitUrl")]
    public string? PortraitUrl { get; set; }

    [JsonProperty("landscapeUrl")]
    public string? LandscapeUrl { get; set; }

    [JsonProperty("mmr")]
    public int? MatchmakingRating { get; set; }

    [JsonProperty("isEligible")]
    public sbyte? CurrentSeasonEligible { get; set; }

    [JsonProperty("score")]
    public int? CurrentSeasonScore { get; set; }

    public virtual ICollection<HenrikPlayerMapping> HenrikPlayerMappings { get; } = new List<HenrikPlayerMapping>();

    public virtual ICollection<PlayerMatch> PlayerMatches { get; } = new List<PlayerMatch>();

    [JsonProperty("rank")]
    public virtual Rank? Rank { get; set; }
}
