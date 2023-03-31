using System;
using System.Collections.Generic;

namespace DoAn;

public partial class FriendRequest
{
    public int ReqId { get; set; }

    public string FromUser { get; set; } = null!;

    public string ToUser { get; set; } = null!;

    public virtual User FromUserNavigation { get; set; } = null!;

    public virtual User ToUserNavigation { get; set; } = null!;
}
