﻿using System;
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

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Share> Shares { get; set; }

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

            entity.ToTable("friend");

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

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("PRIMARY");

            entity.ToTable("likes");

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

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PRIMARY");

            entity.ToTable("posts");

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

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PRIMARY");

            entity.ToTable("refresh_token");

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

            entity.ToTable("share");

            entity.HasIndex(e => e.PostId, "share_post");

            entity.HasIndex(e => e.UserId, "user_share");

            entity.Property(e => e.ShareId)
                .HasColumnType("int(11)")
                .HasColumnName("share_id");
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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");
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
        });

        modelBuilder.Entity<UsersInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users_info");

            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");
            entity.Property(e => e.AnhBia)
                .HasColumnType("mediumtext")
                .HasColumnName("anh_bia");
            entity.Property(e => e.Avatar)
                .HasColumnType("mediumtext")
                .HasColumnName("avatar");
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
                .HasConstraintName("user_in4");
        });

        modelBuilder.Entity<UsersRela>(entity =>
        {
            entity.HasKey(e => e.RelaId).HasName("PRIMARY");

            entity.ToTable("users_rela");

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
