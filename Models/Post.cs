using System;
using System.Collections.Generic;

namespace DoAn;

public partial class Post
{
    public int PostId { get; set; }

    public int UserId { get; set; }

    public string? Content { get; set; }

    public string? Image1 { get; set; }

    public string? Image2 { get; set; }

    public string? Image3 { get; set; }

    public string AccessModifier { get; set; } = null!;

    public int LikeCount { get; set; }

    public int CmCount { get; set; }

    public int SharedPostId { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<Like> Likes { get; } = new List<Like>();

    public virtual User User { get; set; } = null!;
}
