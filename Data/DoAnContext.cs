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

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersInfo> UsersInfos { get; set; }

    public virtual DbSet<UsersRela> UsersRelas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CmId).HasName("PRIMARY");

            entity.ToTable("comment");

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
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comment_post");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_comment");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.FriendId).HasName("PRIMARY");

            entity.ToTable("friend");

            entity.HasIndex(e => e.AddFriend, "add_friend");

            entity.HasIndex(e => e.UserId, "isme");

            entity.Property(e => e.FriendId)
                .HasColumnType("int(11)")
                .HasColumnName("friend_id");
            entity.Property(e => e.AddFriend)
                .HasColumnType("int(11)")
                .HasColumnName("add_friend");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.AddFriendNavigation).WithMany(p => p.FriendAddFriendNavigations)
                .HasForeignKey(d => d.AddFriend)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("add_friend");

            entity.HasOne(d => d.User).WithMany(p => p.FriendUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("isme");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("PRIMARY");

            entity.ToTable("likes");

            entity.HasIndex(e => e.PostId, "post_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.LikeId)
                .HasColumnType("int(11)")
                .HasColumnName("like_id");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.Likes)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("likes_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("likes_ibfk_1");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PRIMARY");

            entity.ToTable("posts");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.AccessModifier)
                .HasMaxLength(127)
                .HasColumnName("access_modifier");
            entity.Property(e => e.CmCount)
                .HasColumnType("int(11)")
                .HasColumnName("cm_count");
            entity.Property(e => e.Content)
                .HasMaxLength(3000)
                .HasColumnName("content");
            entity.Property(e => e.Image1)
                .HasMaxLength(300)
                .HasColumnName("image1");
            entity.Property(e => e.Image2)
                .HasMaxLength(300)
                .HasColumnName("image2");
            entity.Property(e => e.Image3)
                .HasMaxLength(300)
                .HasColumnName("image3");
            entity.Property(e => e.LikeCount)
                .HasColumnType("int(11)")
                .HasColumnName("like_count");
            entity.Property(e => e.SharedPostId)
                .HasColumnType("int(11)")
                .HasColumnName("shared_post_id");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("posts_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnName("avatar_url");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(127)
                .HasColumnName("full_name");
            entity.Property(e => e.Pass)
                .HasMaxLength(100)
                .HasColumnName("pass");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<UsersInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users_info");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Favorites)
                .HasMaxLength(1023)
                .HasColumnName("favorites");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.OtherInfo)
                .HasMaxLength(1023)
                .HasColumnName("other_info");
            entity.Property(e => e.Sex)
                .HasMaxLength(5)
                .HasColumnName("sex");
            entity.Property(e => e.StudyAt)
                .HasMaxLength(127)
                .HasColumnName("study_at");
            entity.Property(e => e.WorkingAt)
                .HasMaxLength(127)
                .HasColumnName("working_at");

            entity.HasOne(d => d.User).WithOne(p => p.UsersInfo)
                .HasForeignKey<UsersInfo>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_info_ibfk_1");
        });

        modelBuilder.Entity<UsersRela>(entity =>
        {
            entity.HasKey(e => e.RelaId).HasName("PRIMARY");

            entity.ToTable("users_rela");

            entity.HasIndex(e => e.UserId, "users_folow");

            entity.Property(e => e.RelaId)
                .ValueGeneratedOnAdd()
                .HasColumnType("int(11)")
                .HasColumnName("rela_id");
            entity.Property(e => e.Follwing)
                .HasColumnType("int(11)")
                .HasColumnName("follwing");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Rela).WithOne(p => p.UsersRelaRela)
                .HasForeignKey<UsersRela>(d => d.RelaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("folow_user");

            entity.HasOne(d => d.User).WithMany(p => p.UsersRelaUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_folow");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
