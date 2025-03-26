using System;
using System.Collections.Generic;

namespace MyProject.Models;

public partial class ForumComments
{
    public string CommentID { get; set; } = null!;

    public string PostID { get; set; } = null!;

    public string ForumID { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public DateTime PostTime { get; set; }

    public string PostContents { get; set; } = null!;

    public virtual ForumName? Forum { get; set; } = null!;

    public virtual Member? Member { get; set; } = null!;

    public virtual Forum? Post { get; set; } = null!;
}
