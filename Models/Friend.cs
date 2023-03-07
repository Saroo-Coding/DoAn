using System;
using System.Collections.Generic;

namespace DoAn;

public partial class Friend
{
    public int FriendId { get; set; }

    public string UserId { get; set; } = null!;

    public string AddFriend { get; set; } = null!;

    public virtual User AddFriendNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
