using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class PlayerMatch
{
    public int PlayerId { get; set; }

    public int MatchId { get; set; }

    public int? AgentId { get; set; }

    public int? Score { get; set; }

    public int? Kills { get; set; }

    public int? Deaths { get; set; }

    public int? Assists { get; set; }

    public int? Dealt { get; set; }

    public int? Received { get; set; }

    public int? BodyShots { get; set; }

    public int? LegShots { get; set; }

    public int? HeadShots { get; set; }

    public virtual Agent? Agent { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}
