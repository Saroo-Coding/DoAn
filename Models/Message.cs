using System;
using System.Collections.Generic;

namespace DoAn;

public partial class Message
{
    public int Id { get; set; }

    public string FromUser { get; set; } = null!;

    public string ToUser { get; set; } = null!;

    public string Message1 { get; set; }= null!;

    public virtual User? FromUserNavigation { get; set; }

    public virtual User? ToUserNavigation { get; set; }
}
