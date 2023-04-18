using System;
using System.Collections.Generic;

namespace DoAn;

public partial class GroupMember
{
    public int MemberId { get; set; }

    public int GroupId { get; set; }

    public string UserId { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
