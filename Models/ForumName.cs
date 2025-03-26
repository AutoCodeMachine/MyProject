using System;
using System.Collections.Generic;

namespace MyProject.Models;

public partial class ForumName
{
    public string ForumID { get; set; } = null!;

    public string ForumName1 { get; set; } = null!;

    public virtual ICollection<Forum> Forum { get; set; } = new List<Forum>();

    public virtual ICollection<ForumComments> ForumComments { get; set; } = new List<ForumComments>();

    public virtual ICollection<ForumHikingGroup> ForumHikingGroup { get; set; } = new List<ForumHikingGroup>();
}
