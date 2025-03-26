using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class RouteLevelData
    {
        [Key]
        public string RouteLevel1 { get; set; } = null!;

        public string? LevelDescription { get; set; }

        public string? HikingSuggestion { get; set; }
    }

    [MetadataType(typeof(RouteLevelData))]
    public partial class RouteLevel
    {
    }

    public class RouteLandmarkTypeData
    {
        [Key]
        public string TypeID { get; set; } = null!;

        public string? TypeName { get; set; }
    }

    [MetadataType(typeof(RouteLandmarkTypeData))]
    public partial class RouteLandmarkType
    {
    }

    public class RouteInformationData
    {
        [Key]
        public string RouteID { get; set; } = null!;

        public string RouteName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Parking { get; set; }

        public bool PermitOfNationalPark { get; set; }

        public bool PermitOfPolice { get; set; }
    }

    [MetadataType(typeof(RouteInformationData))]
    public partial class RouteInformation
    {
    }

    public class RouteLandmarksData
    {
        [Key]
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
    }

    [MetadataType(typeof(RouteLandmarksData))]
    public partial class RouteLandmarks
    {
    }

    public class RouteDetailData
    {
        public int RouteMarkOrder { get; set; }

        public int? BranchPoint { get; set; }

        public int? BranchNumber { get; set; }
    }

    [MetadataType(typeof(RouteDetailData))]
    public partial class RouteDetail
    {
    }
}
