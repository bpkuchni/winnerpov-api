
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WinnerPOV_API.Database;

public partial class PlayerMatch
{
    [JsonIgnore]
    public int PlayerId { get; set; }

    [JsonIgnore]
    public int MatchId { get; set; }

    [JsonIgnore]
    public int? AgentId { get; set; }

    [JsonPropertyName("score")]
    public int? Score { get; set; }

    [JsonPropertyName("kills")]
    public int? Kills { get; set; }

    [JsonPropertyName("deaths")]
    public int? Deaths { get; set; }

    [JsonPropertyName("assists")]
    public int? Assists { get; set; }

    [JsonPropertyName("dealt")]
    public int? Dealt { get; set; }

    [JsonPropertyName("received")]
    public int? Received { get; set; }

    [JsonPropertyName("bodyShots")]
    public int? BodyShots { get; set; }

    [JsonPropertyName("legShots")]
    public int? LegShots { get; set; }

    [JsonPropertyName("headShots")]
    public int? HeadShots { get; set; }

    [JsonPropertyName("agent")]
    public virtual Agent? Agent { get; set; }

    [JsonIgnore]
    public virtual Match Match { get; set; } = null!;

    [JsonPropertyName("player")]
    public virtual Player Player { get; set; } = null!;
}
