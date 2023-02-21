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

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("likes");

            entity.HasIndex(e => e.PostId, "post_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Post).WithMany(p => p.Likes)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("likes_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("likes_ibfk_1");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("posts");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AccessModifier)
                .HasMaxLength(127)
                .HasColumnName("access_modifier");
            entity.Property(e => e.Content)
                .HasMaxLength(3000)
                .HasColumnName("content");
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
                .HasConstraintName("posts_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnName("avatar_url");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(127)
                .HasColumnName("full_name");
            entity.Property(e => e.InterestedUser)
                .HasMaxLength(258)
                .HasColumnName("interested_User");
        });

        modelBuilder.Entity<UsersInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users_info");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
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
            entity.Property(e => e.StudyAt)
                .HasMaxLength(127)
                .HasColumnName("study_at");
            entity.Property(e => e.WorkingAt)
                .HasMaxLength(127)
                .HasColumnName("working_at");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.UsersInfo)
                .HasForeignKey<UsersInfo>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_info_ibfk_1");
        });

        modelBuilder.Entity<UsersRela>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users_rela");

            entity.HasIndex(e => e.Follower, "follower");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Follower)
                .HasColumnType("int(11)")
                .HasColumnName("follower");
            entity.Property(e => e.Follwing)
                .HasColumnType("int(11)")
                .HasColumnName("follwing");

            entity.HasOne(d => d.FollowerNavigation).WithMany(p => p.UsersRelas)
                .HasForeignKey(d => d.Follower)
                .HasConstraintName("users_rela_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1; Database=mxh; Integrated Security=true; MultipleActiveResultSets=true; Trusted_Connection=True");
        }
    }
}
