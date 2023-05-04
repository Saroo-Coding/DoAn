using System;
using System.Collections.Generic;

namespace DoAn;

public partial class PostNotify
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int PostId { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
