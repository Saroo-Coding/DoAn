using System;
using System.Collections.Generic;

namespace DoAn;

public partial class Friend
{
    public int FriendId { get; set; }

    public int UserId { get; set; }

    public int AddFriend { get; set; }

    public virtual User AddFriendNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
