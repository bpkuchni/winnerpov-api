using System;
using System.Collections.Generic;

namespace WinnerPOV_API.Database;

public partial class Agent
{
    public int AgentId { get; set; }

    public string Name { get; set; } = null!;

    public string? ThumbnailUrl { get; set; }

    public string? PortraitUrl { get; set; }

    public virtual ICollection<PlayerMatch> PlayerMatches { get; } = new List<PlayerMatch>();
}
