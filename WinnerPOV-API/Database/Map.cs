using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class Map
{
    public int MapId { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public virtual ICollection<Match> Matches { get; } = new List<Match>();
}
