using System;
using System.Collections.Generic;
using DoAn;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Data;

public partial class DoAnContext : DbContext
{
    public DoAnContext(DbContextOptions<DoAnContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CmtGroupPost> CmtGroupPosts { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<FriendRequest> FriendRequests { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupMember> GroupMembers { get; set; }

    public virtual DbSet<GroupPost> GroupPosts { get; set; }

    public virtual DbSet<JoinGroupReq> JoinGroupReqs { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<LikeGroupPost> LikeGroupPosts { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostNotify> PostNotifies { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Share> Shares { get; set; }

    public virtual DbSet<SharePostGroup> SharePostGroups { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersInfo> UsersInfos { get; set; }

    public virtual DbSet<UsersRela> UsersRelas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<CmtGroupPost>(entity =>
        {
            entity.HasKey(e => e.CmtId).HasName("PRIMARY");

            entity
                .ToTable("cmt_group_post")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.PostId, "cmt_group");

            entity.HasIndex(e => e.UserId, "user_cmt_grp");

            entity.Property(e => e.CmtId)
                .HasColumnType("int(11)")
                .HasColumnName("cmt_id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.CmtGroupPosts)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("cmt_group");

            entity.HasOne(d => d.User).WithMany(p => p.CmtGroupPosts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_cmt_grp");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CmId).HasName("PRIMARY");

            entity
                .ToTable("comment")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.PostId, "comment_post");

            entity.HasIndex(e => e.UserId, "user_comment");

            entity.Property(e => e.CmId)
                .HasColumnType("int(11)")
                .HasColumnName("cm_id");
            entity.Property(e => e.Content)
                .HasMaxLength(3000)
                .HasColumnName("content");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("comment_post");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_comment");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.FriendId).HasName("PRIMARY");

            entity
                .ToTable("friend")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.AddFriend, "add_friends");

            entity.HasIndex(e => e.UserId, "friend_add");

            entity.Property(e => e.FriendId)
                .HasColumnType("int(11)")
                .HasColumnName("friend_id");
            entity.Property(e => e.AddFriend)
                .HasMaxLength(30)
                .HasColumnName("add_friend");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.AddFriendNavigation).WithMany(p => p.FriendAddFriendNavigations)
                .HasForeignKey(d => d.AddFriend)
                .HasConstraintName("add_friends");

            entity.HasOne(d => d.User).WithMany(p => p.FriendUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("friend_add");
        });

        modelBuilder.Entity<FriendRequest>(entity =>
        {
            entity.HasKey(e => e.ReqId).HasName("PRIMARY");

            entity
                .ToTable("friend_request")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.FromUser, "from_user");

            entity.HasIndex(e => e.ToUser, "to_user");

            entity.Property(e => e.ReqId)
                .HasColumnType("int(11)")
                .HasColumnName("req_id");
            entity.Property(e => e.FromUser)
                .HasMaxLength(30)
                .HasColumnName("from_user");
            entity.Property(e => e.ToUser)
                .HasMaxLength(30)
                .HasColumnName("to_user");

            entity.HasOne(d => d.FromUserNavigation).WithMany(p => p.FriendRequestFromUserNavigations)
                .HasForeignKey(d => d.FromUser)
                .HasConstraintName("from_user");

            entity.HasOne(d => d.ToUserNavigation).WithMany(p => p.FriendRequestToUserNavigations)
                .HasForeignKey(d => d.ToUser)
                .HasConstraintName("to_user");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PRIMARY");

            entity
                .ToTable("groups")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.GroupId)
                .HasColumnType("int(11)")
                .HasColumnName("group_id");
            entity.Property(e => e.Avatar)
                .HasColumnType("mediumtext")
                .HasColumnName("avatar");
            entity.Property(e => e.CoverImage)
                .HasColumnType("mediumtext")
                .HasColumnName("cover_image");
            entity.Property(e => e.Intro)
                .HasColumnType("text")
                .HasColumnName("intro");
            entity.Property(e => e.NameGroup)
                .HasColumnType("text")
                .HasColumnName("name_group");
            entity.Property(e => e.StatusGroup)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("status_group");
        });

        modelBuilder.Entity<GroupMember>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PRIMARY");

            entity
                .ToTable("group_member")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.GroupId, "join_group");

            entity.HasIndex(e => e.UserId, "user_join");

            entity.Property(e => e.MemberId)
                .HasColumnType("int(11)")
                .HasColumnName("member_id");
            entity.Property(e => e.GroupId)
                .HasColumnType("int(11)")
                .HasColumnName("group_id");
            entity.Property(e => e.Position)
                .HasMaxLength(7)
                .HasDefaultValueSql("'member'")
                .HasColumnName("position");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupMembers)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("join_group");

            entity.HasOne(d => d.User).WithMany(p => p.GroupMembers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_join");
        });

        modelBuilder.Entity<GroupPost>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PRIMARY");

            entity
                .ToTable("group_post")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.GroupId, "post_group");

            entity.HasIndex(e => e.UserId, "user_group_post");

            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.Content)
                .HasColumnType("mediumtext")
                .HasColumnName("content");
            entity.Property(e => e.DatePost).HasColumnName("date_post");
            entity.Property(e => e.GroupId)
                .HasColumnType("int(11)")
                .HasColumnName("group_id");
            entity.Property(e => e.Img1)
                .HasColumnType("mediumtext")
                .HasColumnName("img1");
            entity.Property(e => e.Img2)
                .HasColumnType("mediumtext")
                .HasColumnName("img2");
            entity.Property(e => e.Img3)
                .HasColumnType("mediumtext")
                .HasColumnName("img3");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupPosts)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("post_group");

            entity.HasOne(d => d.User).WithMany(p => p.GroupPosts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_group_post");
        });

        modelBuilder.Entity<JoinGroupReq>(entity =>
        {
            entity.HasKey(e => e.ReqId).HasName("PRIMARY");

            entity
                .ToTable("join_group_req")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.GroupId, "req_group");

            entity.HasIndex(e => e.UserId, "user_req");

            entity.Property(e => e.ReqId)
                .HasColumnType("int(11)")
                .HasColumnName("req_id");
            entity.Property(e => e.GroupId)
                .HasColumnType("int(11)")
                .HasColumnName("group_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Group).WithMany(p => p.JoinGroupReqs)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("req_group");

            entity.HasOne(d => d.User).WithMany(p => p.JoinGroupReqs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_req");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("PRIMARY");

            entity
                .ToTable("likes")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.PostId, "post_id");

            entity.HasIndex(e => e.UserId, "user_like");

            entity.Property(e => e.LikeId)
                .HasColumnType("int(11)")
                .HasColumnName("like_id");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.Likes)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("user_post");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_like");
        });

        modelBuilder.Entity<LikeGroupPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("like_group_post")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.PostId, "group_post");

            entity.HasIndex(e => e.UserId, "member_like");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.LikeGroupPosts)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("group_post");

            entity.HasOne(d => d.User).WithMany(p => p.LikeGroupPosts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("member_like");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PRIMARY");

            entity
                .ToTable("posts")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.UserId, "my_post");

            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.AccessModifier)
                .HasMaxLength(10)
                .HasColumnName("access_modifier");
            entity.Property(e => e.Content)
                .HasColumnType("mediumtext")
                .HasColumnName("content");
            entity.Property(e => e.DatePost).HasColumnName("date_post");
            entity.Property(e => e.Image1)
                .HasColumnType("mediumtext")
                .HasColumnName("image1");
            entity.Property(e => e.Image2)
                .HasColumnType("mediumtext")
                .HasColumnName("image2");
            entity.Property(e => e.Image3)
                .HasColumnType("mediumtext")
                .HasColumnName("image3");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("my_post");
        });

        modelBuilder.Entity<PostNotify>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("post_notify");

            entity.HasIndex(e => e.FromUser, "notify_to_user");

            entity.HasIndex(e => e.PostId, "post_notification");

            entity.HasIndex(e => e.ToUser, "user_send");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.FromUser)
                .HasMaxLength(30)
                .HasColumnName("from_user")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.ToUser)
                .HasMaxLength(30)
                .HasColumnName("to_user")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            entity.HasOne(d => d.FromUserNavigation).WithMany(p => p.PostNotifyFromUserNavigations)
                .HasForeignKey(d => d.FromUser)
                .HasConstraintName("notify_to_user");

            entity.HasOne(d => d.Post).WithMany(p => p.PostNotifies)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("post_notification");

            entity.HasOne(d => d.ToUserNavigation).WithMany(p => p.PostNotifyToUserNavigations)
                .HasForeignKey(d => d.ToUser)
                .HasConstraintName("user_send");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PRIMARY");

            entity
                .ToTable("refresh_token")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.UserId, "user_token");

            entity.Property(e => e.TokenId)
                .HasColumnType("int(11)")
                .HasColumnName("token_id");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("datetime")
                .HasColumnName("expiry_date");
            entity.Property(e => e.Token)
                .HasMaxLength(200)
                .HasColumnName("token");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_token");
        });

        modelBuilder.Entity<Share>(entity =>
        {
            entity.HasKey(e => e.ShareId).HasName("PRIMARY");

            entity
                .ToTable("share")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.PostId, "share_post");

            entity.HasIndex(e => e.UserId, "user_share");

            entity.Property(e => e.ShareId)
                .HasColumnType("int(11)")
                .HasColumnName("share_id");
            entity.Property(e => e.DateShare)
                .HasColumnType("datetime")
                .HasColumnName("date_share");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.Shares)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("share_post");

            entity.HasOne(d => d.User).WithMany(p => p.Shares)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_share");
        });

        modelBuilder.Entity<SharePostGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("share_post_group");

            entity.HasIndex(e => e.PostId, "chiase_post");

            entity.HasIndex(e => e.GroupId, "from_group");

            entity.HasIndex(e => e.UserId, "user_chiase");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.DateShare)
                .HasColumnType("datetime")
                .HasColumnName("date_share");
            entity.Property(e => e.GroupId)
                .HasColumnType("int(11)")
                .HasColumnName("group_id");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            entity.HasOne(d => d.Group).WithMany(p => p.SharePostGroups)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("from_group");

            entity.HasOne(d => d.Post).WithMany(p => p.SharePostGroups)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("chiase_post");

            entity.HasOne(d => d.User).WithMany(p => p.SharePostGroups)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_chiase");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity
                .ToTable("users")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");
            entity.Property(e => e.BirthDay).HasColumnName("birth_day");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(127)
                .HasColumnName("full_name");
            entity.Property(e => e.Pass)
                .HasMaxLength(50)
                .HasColumnName("pass");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
            entity.Property(e => e.Sex)
                .HasMaxLength(5)
                .HasColumnName("sex");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<UsersInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity
                .ToTable("users_info")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");
            entity.Property(e => e.AnhBia)
                .HasColumnType("mediumtext")
                .HasColumnName("anh_bia");
            entity.Property(e => e.Avatar)
                .HasColumnType("mediumtext")
                .HasColumnName("avatar");
            entity.Property(e => e.Favorites)
                .HasMaxLength(1023)
                .HasColumnName("favorites");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.OtherInfo)
                .HasMaxLength(1023)
                .HasColumnName("other_info");
            entity.Property(e => e.StudyAt)
                .HasMaxLength(127)
                .HasColumnName("study_at");
            entity.Property(e => e.WorkingAt)
                .HasMaxLength(127)
                .HasColumnName("working_at");

            entity.HasOne(d => d.User).WithOne(p => p.UsersInfo)
                .HasForeignKey<UsersInfo>(d => d.UserId)
                .HasConstraintName("user_in4");
        });

        modelBuilder.Entity<UsersRela>(entity =>
        {
            entity.HasKey(e => e.RelaId).HasName("PRIMARY");

            entity
                .ToTable("users_rela")
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.UserId, "follow_me");

            entity.HasIndex(e => e.Follwing, "me_follow");

            entity.Property(e => e.RelaId)
                .HasColumnType("int(11)")
                .HasColumnName("rela_id");
            entity.Property(e => e.Follwing)
                .HasMaxLength(30)
                .HasColumnName("follwing");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.FollwingNavigation).WithMany(p => p.UsersRelaFollwingNavigations)
                .HasForeignKey(d => d.Follwing)
                .HasConstraintName("me_follow");

            entity.HasOne(d => d.User).WithMany(p => p.UsersRelaUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("follow_me");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
