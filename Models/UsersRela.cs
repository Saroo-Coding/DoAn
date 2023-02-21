using System;
using System.Collections.Generic;

namespace DoAn;

public partial class UsersRela
{
    public int Id { get; set; }

    public int? Follower { get; set; }

    public int? Follwing { get; set; }

    public virtual User? FollowerNavigation { get; set; }
}
