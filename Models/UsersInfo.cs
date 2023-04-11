using System;
using System.Collections.Generic;

namespace DoAn;

public partial class UsersInfo
{
    public string? UserId { get; set; }

    public string? Avatar { get; set; }

    public string? AnhBia { get; set; }

    public bool? IsActive { get; set; }

    public string? StudyAt { get; set; }

    public string? WorkingAt { get; set; }

    public string? Favorites { get; set; }

    public string? OtherInfo { get; set; }

    public virtual User? User { get; set; }
}
