using System;
using System.Collections.Generic;

namespace MyProject.Models;

public partial class RouteLandmarks
{
    public string TypeID { get; set; } = null!;

    public string LandmarkID { get; set; } = null!;

    public string LandmarkName { get; set; } = null!;

    public int Altitude { get; set; }

    public decimal Distance { get; set; }

    public int HikingTimeUp { get; set; }

    public int HikingTimeDown { get; set; }

    public string? NickName { get; set; }

    public string? WaterSource { get; set; }

    public string? Description { get; set; }

    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public virtual ICollection<HikingRecords>? HikingRecords { get; set; } = new List<HikingRecords>();

    public virtual ICollection<RouteDetail>? RouteDetail { get; set; } = new List<RouteDetail>();

    public virtual RouteLandmarkType? Type { get; set; } = null!;
}
