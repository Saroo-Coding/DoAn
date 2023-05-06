﻿using System;
using System.Collections.Generic;

namespace DoAn;

public partial class JoinGroupReq
{
    public int ReqId { get; set; }

    public int GroupId { get; set; }

    public string UserId { get; set; } = null!;

    public virtual Group? Group { get; set; }

    public virtual User? User { get; set; }
}
