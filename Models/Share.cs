﻿using System;
using System.Collections.Generic;

namespace DoAn;

public partial class Share
{
    public int ShareId { get; set; }

    public string UserId { get; set; } = null!;

    public int PostId { get; set; }

    public DateTime DateShare { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
