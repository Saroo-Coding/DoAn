using System;
using System.Collections.Generic;

namespace DoAn;

public partial class UsersInfo
{
    public int Id { get; set; }

    public bool? IsActive { get; set; }

    public string? StudyAt { get; set; }

    public string? WorkingAt { get; set; }

    public string? Favorites { get; set; }

    public string? OtherInfo { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User IdNavigation { get; set; } = null!;
}
