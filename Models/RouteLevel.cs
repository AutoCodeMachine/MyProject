using System;
using System.Collections.Generic;

namespace MyProject.Models;

public partial class RouteLevel
{
    public string RouteLevel1 { get; set; } = null!;

    public string? LevelDescription { get; set; }

    public string? HikingSuggestion { get; set; }

    public virtual ICollection<RouteDetail> RouteDetail { get; set; } = new List<RouteDetail>();
}
