using System;
using System.Collections.Generic;

namespace DoAn;

public partial class Group
{
    public int GroupId { get; set; }

    public bool? StatusGroup { get; set; }

    public string NameGroup { get; set; } = null!;

    public string Avatar { get; set; } = null!;

    public string CoverImage { get; set; } = null!;

    public string Intro { get; set; } = null!;

    public virtual ICollection<GroupMember> GroupMembers { get; } = new List<GroupMember>();

    public virtual ICollection<GroupPost> GroupPosts { get; } = new List<GroupPost>();

    public virtual ICollection<JoinGroupReq> JoinGroupReqs { get; } = new List<JoinGroupReq>();

    public virtual ICollection<SharePostGroup> SharePostGroups { get; } = new List<SharePostGroup>();
}
