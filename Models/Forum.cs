using System;
using System.Collections.Generic;

namespace MyProject.Models;

public partial class Forum
{
    public string PostID { get; set; } = null!;

    public string ForumID { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public DateTime PostTime { get; set; }

    public string PostTitle { get; set; } = null!;

    public string PostContents { get; set; } = null!;

    public virtual ICollection<ForumComments> ForumComments { get; set; } = new List<ForumComments>();

    public virtual ICollection<ForumImage> ForumImage { get; set; } = new List<ForumImage>();

    public virtual ForumName ForumName { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
