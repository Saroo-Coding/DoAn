using System;
using System.Collections.Generic;

namespace DoAn;

public partial class UsersInfo
{
    public int UserId { get; set; }

    public string Sex { get; set; } = null!;

    public bool? IsActive { get; set; }

    public string? StudyAt { get; set; }

    public string? WorkingAt { get; set; }

    public string? Favorites { get; set; }

    public string? OtherInfo { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
