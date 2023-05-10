using System;
using System.Collections.Generic;

namespace DoAn;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public DateOnly BirthDay { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<CmtGroupPost> CmtGroupPosts { get; } = new List<CmtGroupPost>();

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<Friend> FriendAddFriendNavigations { get; } = new List<Friend>();

    public virtual ICollection<FriendRequest> FriendRequestFromUserNavigations { get; } = new List<FriendRequest>();

    public virtual ICollection<FriendRequest> FriendRequestToUserNavigations { get; } = new List<FriendRequest>();

    public virtual ICollection<Friend> FriendUsers { get; } = new List<Friend>();

    public virtual ICollection<GroupMember> GroupMembers { get; } = new List<GroupMember>();

    public virtual ICollection<GroupPost> GroupPosts { get; } = new List<GroupPost>();

    public virtual ICollection<JoinGroupReq> JoinGroupReqs { get; } = new List<JoinGroupReq>();

    public virtual ICollection<LikeGroupPost> LikeGroupPosts { get; } = new List<LikeGroupPost>();

    public virtual ICollection<Like> Likes { get; } = new List<Like>();

    public virtual ICollection<PostNotify> PostNotifyFromUserNavigations { get; } = new List<PostNotify>();

    public virtual ICollection<PostNotify> PostNotifyToUserNavigations { get; } = new List<PostNotify>();

    public virtual ICollection<Post> Posts { get; } = new List<Post>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();

    public virtual ICollection<SharePostGroup> SharePostGroups { get; } = new List<SharePostGroup>();

    public virtual ICollection<Share> Shares { get; } = new List<Share>();

    public virtual UsersInfo? UsersInfo { get; set; }

    public virtual ICollection<UsersRela> UsersRelaFollwingNavigations { get; } = new List<UsersRela>();

    public virtual ICollection<UsersRela> UsersRelaUsers { get; } = new List<UsersRela>();
}
