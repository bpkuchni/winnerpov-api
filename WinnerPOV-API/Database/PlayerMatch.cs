using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class PlayerMatch
{
    public int PlayerId { get; set; }

    public int MatchId { get; set; }

    public int? AgentId { get; set; }

    [JsonProperty("score")]
    public int? Score { get; set; }

    [JsonProperty("kills")]
    public int? Kills { get; set; }

    [JsonProperty("deaths")]
    public int? Deaths { get; set; }

    [JsonProperty("assists")]
    public int? Assists { get; set; }

    [JsonProperty("dealt")]
    public int? Dealt { get; set; }

    [JsonProperty("received")]
    public int? Received { get; set; }

    [JsonProperty("bodyShots")]
    public int? BodyShots { get; set; }

    [JsonProperty("legShots")]
    public int? LegShots { get; set; }

    [JsonProperty("headShots")]
    public int? HeadShots { get; set; }

    [JsonProperty("agent")]
    public virtual Agent? Agent { get; set; }

    public virtual Match Match { get; set; } = null!;

    [JsonProperty("player")]
    public virtual Player Player { get; set; } = null!;
}
