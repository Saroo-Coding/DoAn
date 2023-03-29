using System;
using System.Collections.Generic;

namespace DoAn;

public partial class UsersInfo
{
    public string UserId { get; set; } = null!;

    public string Avatar { get; set; } = null!;

    public string AnhBia { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public bool? IsActive { get; set; }

    public string? StudyAt { get; set; }

    public string? WorkingAt { get; set; }

    public string? Favorites { get; set; }

    public string? OtherInfo { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public virtual User User { get; set; } = null!;
}
