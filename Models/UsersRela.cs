using System;
using System.Collections.Generic;

namespace DoAn;

public partial class UsersRela
{
    public int RelaId { get; set; }

    public string UserId { get; set; } = null!;

    public string Follwing { get; set; } = null!;

    public virtual User FollwingNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
