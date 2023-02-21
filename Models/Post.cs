using System;
using System.Collections.Generic;

namespace DoAn;

public partial class Post
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Content { get; set; }

    public string? AccessModifier { get; set; }

    public int? LikeCount { get; set; }

    public int? SharedPostId { get; set; }

    public virtual ICollection<Like> Likes { get; } = new List<Like>();

    public virtual User? User { get; set; }
}
