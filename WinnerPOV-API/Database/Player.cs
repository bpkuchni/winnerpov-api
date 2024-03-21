using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class Player
{
    public int PlayerId { get; set; }

    public int? RankId { get; set; }

    public string Name { get; set; } = null!;

    public string Tag { get; set; } = null!;

    public int? Level { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? PortraitUrl { get; set; }

    public string? LandscapeUrl { get; set; }

    public int? MatchmakingRating { get; set; }

    public sbyte? CurrentSeasonEligible { get; set; }

    public int? CurrentSeasonScore { get; set; }

    public virtual ICollection<HenrikPlayerMapping> HenrikPlayerMappings { get; } = new List<HenrikPlayerMapping>();

    public virtual ICollection<PlayerMatch> PlayerMatches { get; } = new List<PlayerMatch>();

    public virtual Rank? Rank { get; set; }
}
