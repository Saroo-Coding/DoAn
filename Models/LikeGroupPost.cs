﻿using System;
using System.Collections.Generic;

namespace DoAn;

public partial class LikeGroupPost
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int PostId { get; set; }

    public virtual GroupPost? Post { get; set; }

    public virtual User? User { get; set; }
}
