using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace MyProject.Models;

public partial class Member
{
    public string MemberID { get; set; } = null!;

    public string PersonalID { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string PersonalName { get; set; } = null!;

    public bool Gender { get; set; }

    public DateOnly Birthday { get; set; }

    public string Mobile { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Forum> Forum { get; set; } = new List<Forum>();

    public virtual ICollection<ForumComments> ForumComments { get; set; } = new List<ForumComments>();

    public virtual ICollection<ForumHikingGroup> ForumHikingGroup { get; set; } = new List<ForumHikingGroup>();

    public virtual ICollection<HikingRecords> HikingRecords { get; set; } = new List<HikingRecords>();

    [JsonIgnore]
    public virtual MemberPassword? MemberPassword { get; set; }
}
