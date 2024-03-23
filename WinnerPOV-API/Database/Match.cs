
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WinnerPOV_API.Database;

public partial class Match
{
  
    public int MatchId { get; set; }

    [JsonIgnore]
    public int? MapId { get; set; }

    [JsonPropertyName("ourScore")]
    public int? OurScore { get; set; }

    [JsonPropertyName("theirScore")]
    public int? TheirScore { get; set; }

    [JsonPropertyName("opponentName")]
    public string? OpponentName { get; set; }

    [JsonPropertyName("opponentTag")]
    public string? OpponentTag { get; set; }

    [JsonPropertyName("opponentImageUrl")]
    public string? OpponentImageUrl { get; set; }

    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("rounds")]
    public int? Rounds { get; set; }

    [JsonPropertyName("videoEmbedUrl")]
    public string? VideoEmbedUrl { get; set; }

    [JsonIgnore]
    public virtual ICollection<HenrikMatchMapping> HenrikMatchMappings { get; } = new List<HenrikMatchMapping>();

    [JsonPropertyName("map")]
    public virtual Map? Map { get; set; }

    [JsonPropertyName("players")]
    public virtual ICollection<PlayerMatch> PlayerMatches { get; } = new List<PlayerMatch>();
}
