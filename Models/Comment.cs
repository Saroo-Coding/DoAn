using System;
using System.Collections.Generic;

namespace DoAn;

public partial class Comment
{
    public int CmId { get; set; }

    public int PostId { get; set; }

    public string UserId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
