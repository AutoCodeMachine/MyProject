using System;
using System.Collections.Generic;

namespace MyProject.Models;

public partial class ForumImage
{
    public string Image { get; set; } = null!;

    public string ImageType { get; set; } = null!;

    public string PostID { get; set; } = null!;

    public virtual Forum Post { get; set; } = null!;
}
