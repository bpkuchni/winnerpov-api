using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class Match
{
    public int MatchId { get; set; }

    public int? MapId { get; set; }

    public int? OurScore { get; set; }

    public int? TheirScore { get; set; }

    public string? OpponentName { get; set; }

    public string? OpponentTag { get; set; }

    public string? OpponentImageUrl { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? Duration { get; set; }

    public int? Rounds { get; set; }

    public virtual ICollection<HenrikMatchMapping> HenrikMatchMappings { get; } = new List<HenrikMatchMapping>();

    public virtual Map? Map { get; set; }

    public virtual ICollection<PlayerMatch> PlayerMatches { get; } = new List<PlayerMatch>();
}
