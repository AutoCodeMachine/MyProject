using System;
using System.Collections.Generic;

namespace MyProject.Models;

public partial class RouteInformation
{
    public string RouteID { get; set; } = null!;

    public string RouteName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Parking { get; set; }

    public bool PermitOfNationalPark { get; set; }

    public bool PermitOfPolice { get; set; }

    public virtual ICollection<RouteDetail>? RouteDetail { get; set; } = new List<RouteDetail>();
}
