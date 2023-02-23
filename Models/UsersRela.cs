using System;
using System.Collections.Generic;

namespace DoAn;

public partial class UsersRela
{
    public int RelaId { get; set; }

    public int? UserId { get; set; }

    public int? Follwing { get; set; }

    public virtual User? User { get; set; }
}
