
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WinnerPOV_API.Database;

public partial class Season
{ 
    public int SeasonId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("start")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("ranking")]
    public int? Ranking { get; set; }

    [JsonPropertyName("wins")]
    public int? Wins { get; set; }

    [JsonPropertyName("losses")]
    public int? Losses { get; set; }

    [JsonPropertyName("score")]
    public int? Score { get; set; }
}
