using System;
using System.Collections.Generic;

namespace MyProject.Models;

public partial class ForumHikingGroup
{
    public string PostID { get; set; } = null!;

    public string ForumID { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public DateTime PostTime { get; set; }

    public string PostTitle { get; set; } = null!;

    public string PostContents { get; set; } = null!;

    public DateTime DepartureTime { get; set; }

    public int MaxMembers { get; set; }

    public DateTime Deadline { get; set; }

    public string JoinMembers { get; set; } = null!;

    public virtual ForumName Forum { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
