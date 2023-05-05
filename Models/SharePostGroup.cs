using System;
using System.Collections.Generic;

namespace DoAn;

public partial class SharePostGroup
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int PostId { get; set; }

    public virtual GroupPost Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
