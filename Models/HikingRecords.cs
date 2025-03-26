using System;
using System.Collections.Generic;

namespace MyProject.Models;

public partial class HikingRecords
{
    public string RecordID { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public DateTime UploadDate { get; set; }

    public string LandmarkID { get; set; } = null!;

    public DateOnly HikingDate { get; set; }

    public TimeOnly HikingTime { get; set; }

    public string? gpxFile { get; set; }

    public string? HikingNote { get; set; }

    public string? ImageName { get; set; }

    public string? ImageType { get; set; }

    public bool PublicPrivate { get; set; }

    public virtual RouteLandmarks Landmark { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
