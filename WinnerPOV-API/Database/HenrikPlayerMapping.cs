using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class HenrikPlayerMapping
{
    public string HenrikId { get; set; } = null!;

    public int PlayerId { get; set; }

    public virtual Player Player { get; set; } = null!;
}
