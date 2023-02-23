using System;
using System.Collections.Generic;

namespace DoAn;

public partial class Like
{
    public int LikeId { get; set; }

    public int? UserId { get; set; }

    public int? PostId { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
