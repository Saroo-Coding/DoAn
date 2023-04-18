using System;
using System.Collections.Generic;

namespace DoAn;

public partial class CmtGroupPost
{
    public int CmtId { get; set; }

    public int PostId { get; set; }

    public string UserId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public virtual GroupPost Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
