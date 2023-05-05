using System;
using System.Collections.Generic;

namespace DoAn;

public partial class GroupPost
{
    public int PostId { get; set; }

    public int GroupId { get; set; }

    public string UserId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string Img1 { get; set; } = null!;

    public string Img2 { get; set; } = null!;

    public string Img3 { get; set; } = null!;

    public DateTime DatePost { get; set; }

    public virtual ICollection<CmtGroupPost> CmtGroupPosts { get; } = new List<CmtGroupPost>();

    public virtual Group? Group { get; set; }

    public virtual ICollection<LikeGroupPost> LikeGroupPosts { get; } = new List<LikeGroupPost>();

    public virtual ICollection<SharePostGroup> SharePostGroups { get; } = new List<SharePostGroup>();

    public virtual User? User { get; set; }
}
