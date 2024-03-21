using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class HenrikMatchMapping
{
    public int HenrikId { get; set; }

    public int MatchId { get; set; }

    public virtual Match Match { get; set; } = null!;
}
