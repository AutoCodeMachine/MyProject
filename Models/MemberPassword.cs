using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyProject.Models;

public partial class MemberPassword
{
    public string MemberID { get; set; } = null!;

    public string AccountPassword { get; set; } = null!;

    [JsonIgnore]
    public virtual Member? Member { get; set; } = null!;
}
