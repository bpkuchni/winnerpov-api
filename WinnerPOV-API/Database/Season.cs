using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class Season
{
    public int SeasonId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int? Ranking { get; set; }

    public int? Wins { get; set; }

    public int? Losses { get; set; }

    public int? Score { get; set; }
}
