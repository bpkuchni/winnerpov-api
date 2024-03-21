using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class HenrikMatchMapping
{
    public string HenrikId { get; set; } = null!;

    public int MatchId { get; set; }

    public virtual Match Match { get; set; } = null!;
}
