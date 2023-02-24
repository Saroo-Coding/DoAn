using System;
using System.Collections.Generic;

namespace DoAn;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public virtual ICollection<Like> Likes { get; } = new List<Like>();

    public virtual ICollection<Post> Posts { get; } = new List<Post>();

    public virtual UsersInfo? UsersInfo { get; set; }

    public virtual ICollection<UsersRela> UsersRelas { get; } = new List<UsersRela>();
}
