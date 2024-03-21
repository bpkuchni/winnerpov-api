using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class Match
{
    public int MatchId { get; set; }

    public int? MapId { get; set; }

    [JsonProperty("ourScore")]
    public int? OurScore { get; set; }

    [JsonProperty("theirScore")]
    public int? TheirScore { get; set; }

    [JsonProperty("opponentName")]
    public string? OpponentName { get; set; }

    [JsonProperty("opponentTag")]
    public string? OpponentTag { get; set; }

    [JsonProperty("opponentImageUrl")]
    public string? OpponentImageUrl { get; set; }

    [JsonProperty("date")]
    public DateTime? Date { get; set; }

    [JsonProperty("duration")]
    public DateTime? Duration { get; set; }

    [JsonProperty("rounds")]
    public int? Rounds { get; set; }

    [JsonProperty("videoEmbedUrl")]
    public string? VideoEmbedUrl { get; set; }

    public virtual ICollection<HenrikMatchMapping> HenrikMatchMappings { get; } = new List<HenrikMatchMapping>();

    [JsonProperty("map")]
    public virtual Map? Map { get; set; }

    [JsonProperty("players")]
    public virtual ICollection<PlayerMatch> PlayerMatches { get; } = new List<PlayerMatch>();
}
