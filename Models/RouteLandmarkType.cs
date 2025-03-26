using System;
using System.Collections.Generic;

namespace MyProject.Models;

public partial class RouteLandmarkType
{
    public string TypeID { get; set; } = null!;

    public string TypeName { get; set; } = null!;

    public virtual ICollection<RouteDetail>? RouteDetail { get; set; } = new List<RouteDetail>();

    public virtual ICollection<RouteLandmarks>? RouteLandmarks { get; set; } = new List<RouteLandmarks>();
}
