using System;
using System.Collections.Generic;

namespace DoAn;

public partial class Post
{
    public int PostId { get; set; }

    public string UserId { get; set; } = null!;

    public string? Content { get; set; }

    public string? Image1 { get; set; }

    public string? Image2 { get; set; }

    public string? Image3 { get; set; }

    public string AccessModifier { get; set; } = null!;

    public DateTime DatePost { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; } = new List<Like>();

    public virtual User? User { get; set; }
}
