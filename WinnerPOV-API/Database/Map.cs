
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WinnerPOV_API.Database;

public partial class Map
{
  
    public int MapId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; }

    [JsonIgnore]
    public virtual ICollection<Match> Matches { get; } = new List<Match>();
}
