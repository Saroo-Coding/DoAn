using System;
using System.Collections.Generic;

namespace DoAn;

public partial class PostNotify
{
    public int Id { get; set; }

    public string FromUser { get; set; } = null!;

    public int PostId { get; set; }

    public string ToUser { get; set; } = null!;

    public string Content { get; set; } = null!;

    public double Status { get; set; }

    public DateTime Date { get; set; }

    public virtual User? FromUserNavigation { get; set; } 

    public virtual Post? Post { get; set; } 

    public virtual User? ToUserNavigation { get; set; } 
}
