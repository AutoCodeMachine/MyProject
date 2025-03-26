using System;
using System.Collections.Generic;

namespace MyProject.Models;

public partial class RouteDetail
{
    public string RouteID { get; set; } = null!;

    public string TypeID { get; set; } = null!;

    public string LandmarkID { get; set; } = null!;

    public string? RouteLevel { get; set; }

    public int RouteMarkOrder { get; set; }

    public int? BranchPoint { get; set; }

    public int? BranchNumber { get; set; }

    public virtual RouteLandmarks Landmark { get; set; } = null!;

    public virtual RouteInformation Route { get; set; } = null!;

    public virtual RouteLevel? RouteLevelID { get; set; }

    public virtual RouteLandmarkType Type { get; set; } = null!;
}
