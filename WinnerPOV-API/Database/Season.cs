using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class Season
{
    public int SeasonId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("start")]
    public DateTime StartDate { get; set; }

    [JsonProperty("end")]
    public DateTime EndDate { get; set; }

    [JsonProperty("ranking")]
    public int? Ranking { get; set; }

    [JsonProperty("wins")]
    public int? Wins { get; set; }

    [JsonProperty("losses")]
    public int? Losses { get; set; }

    [JsonProperty("score")]
    public int? Score { get; set; }
}
