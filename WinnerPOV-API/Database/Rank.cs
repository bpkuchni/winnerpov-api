using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class Rank
{
    public int RankId { get; set; }

    public string Name { get; set; } = null!;

    public string? IconSmallUrl { get; set; }

    public string? IconBigUrl { get; set; }

    public virtual ICollection<Player> Players { get; } = new List<Player>();
}
