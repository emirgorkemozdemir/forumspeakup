using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Voice_Form.Models;

public partial class VoiceFormContext : DbContext
{
    public VoiceFormContext()
    {
    }

    public VoiceFormContext(DbContextOptions<VoiceFormContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TableCategory> TableCategories { get; set; }

    public virtual DbSet<TableComment> TableComments { get; set; }

    public virtual DbSet<TableCommentLike> TableCommentLikes { get; set; }

    public virtual DbSet<TableFollow> TableFollows { get; set; }

    public virtual DbSet<TableLike> TableLikes { get; set; }

    public virtual DbSet<TableNotification> TableNotifications { get; set; }

    public virtual DbSet<TableReport> TableReports { get; set; }

    public virtual DbSet<TableReportComment> TableReportComments { get; set; }

    public virtual DbSet<TableStat> TableStats { get; set; }

    public virtual DbSet<TableSubCategory> TableSubCategories { get; set; }

    public virtual DbSet<TableTopic> TableTopics { get; set; }

    public virtual DbSet<TableUser> TableUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=94.138.197.30;Database=forumspeakupdb;user=emiradmin;password=SelinEmir123!;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TableCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("TableCategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryCount).HasDefaultValueSql("((0))");
            entity.Property(e => e.CategoryName).HasMaxLength(40);
        });

        modelBuilder.Entity<TableComment>(entity =>
        {
            entity.HasKey(e => e.CommentId);

            entity.ToTable("TableComment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CommentActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CommentLike).HasDefaultValueSql("((0))");
            entity.Property(e => e.CommentSenderId).HasColumnName("CommentSenderID");
            entity.Property(e => e.CommentSharerIp)
                .HasMaxLength(15)
                .HasColumnName("CommentSharerIP");
            entity.Property(e => e.CommentTopicId).HasColumnName("CommentTopicID");

            entity.HasOne(d => d.CommentSender).WithMany(p => p.TableComments)
                .HasForeignKey(d => d.CommentSenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableComment_TableUser");
        });

        modelBuilder.Entity<TableCommentLike>(entity =>
        {
            entity.HasKey(e => e.CommentLikeId);

            entity.ToTable("TableCommentLike");

            entity.Property(e => e.CommentLikeId).HasColumnName("CommentLikeID");
            entity.Property(e => e.CommentLikeActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.CommentLikedNavigation).WithMany(p => p.TableCommentLikes)
                .HasForeignKey(d => d.CommentLiked)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableCommentLike_TableComment");

            entity.HasOne(d => d.CommentLikerNavigation).WithMany(p => p.TableCommentLikes)
                .HasForeignKey(d => d.CommentLiker)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableCommentLike_TableUser");
        });

        modelBuilder.Entity<TableFollow>(entity =>
        {
            entity.HasKey(e => e.FollowId);

            entity.ToTable("TableFollow");

            entity.Property(e => e.FollowId).HasColumnName("FollowID");
            entity.Property(e => e.FollowActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.FollowerId).HasColumnName("FollowerID");
            entity.Property(e => e.FollowingSubId).HasColumnName("FollowingSubID");

            entity.HasOne(d => d.Follower).WithMany(p => p.TableFollows)
                .HasForeignKey(d => d.FollowerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableFollow_TableUser");

            entity.HasOne(d => d.FollowingSub).WithMany(p => p.TableFollows)
                .HasForeignKey(d => d.FollowingSubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableFollow_TableSubCategory");
        });

        modelBuilder.Entity<TableLike>(entity =>
        {
            entity.HasKey(e => e.LikeId);

            entity.ToTable("TableLike");

            entity.Property(e => e.LikeId).HasColumnName("LikeID");
            entity.Property(e => e.LikeActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.LikeTopicNavigation).WithMany(p => p.TableLikes)
                .HasForeignKey(d => d.LikeTopic)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableLike_TableTopic");

            entity.HasOne(d => d.LikeUserNavigation).WithMany(p => p.TableLikes)
                .HasForeignKey(d => d.LikeUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableLike_TableUser");
        });

        modelBuilder.Entity<TableNotification>(entity =>
        {
            entity.HasKey(e => e.NotificationId);

            entity.ToTable("TableNotification");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.NotificationDate)
             .HasDefaultValueSql("(getdate())")
             .HasColumnType("datetime");
            entity.Property(e => e.NotificationActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.NotificationContent).HasMaxLength(150);

            entity.HasOne(d => d.NotificationOwnerNavigation).WithMany(p => p.TableNotifications)
                .HasForeignKey(d => d.NotificationOwner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableNotification_TableUser");
        });

        modelBuilder.Entity<TableReport>(entity =>
        {
            entity.HasKey(e => e.ReportId);

            entity.ToTable("TableReport");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.ReportDate)
           .HasDefaultValueSql("(getdate())")
           .HasColumnType("datetime");
            entity.Property(e => e.ReportActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.ReporterNavigation).WithMany(p => p.TableReports)
                .HasForeignKey(d => d.Reporter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableReport_TableUser");

            entity.HasOne(d => d.ReportingNavigation).WithMany(p => p.TableReports)
                .HasForeignKey(d => d.Reporting)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableReport_TableTopic1");
        });

        modelBuilder.Entity<TableReportComment>(entity =>
        {
            entity.HasKey(e => e.ReportCommentId);

            entity.ToTable("TableReportComment");

            entity.Property(e => e.ReportCommentId).HasColumnName("ReportCommentID");

            entity.Property(e => e.ReportCommentDate)
          .HasDefaultValueSql("(getdate())")
          .HasColumnType("datetime");

            entity.Property(e => e.ReportCommentActive)
              .IsRequired()
              .HasColumnName("ReportCommentActive")
              .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.ReportCommentReporterNavigation).WithMany(p => p.TableReportComments)
                .HasForeignKey(d => d.ReportCommentReporter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableReportComment_TableUser");

            entity.HasOne(d => d.ReportCommentReportingNavigation).WithMany(p => p.TableReportComments)
                .HasForeignKey(d => d.ReportCommentReporting)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableReportComment_TableComment");
        });

        modelBuilder.Entity<TableStat>(entity =>
        {
            entity.HasKey(e => e.StatId);

            entity.ToTable("TableStat");

            entity.Property(e => e.StatId).HasColumnName("StatID");
            entity.Property(e => e.StatDate)
           .HasDefaultValueSql("(getdate())")
           .HasColumnType("datetime");

            entity.HasOne(d => d.StatBestTopicNavigation).WithMany(p => p.TableStats)
                .HasForeignKey(d => d.StatBestTopic)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableStat_TableTopic");
        });

        modelBuilder.Entity<TableSubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCategoryId);

            entity.ToTable("TableSubCategory");

            entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");
            entity.Property(e => e.SubCategoryMainId).HasColumnName("SubCategoryMainID");
            entity.Property(e => e.SubCategoryName).HasMaxLength(50);

            entity.HasOne(d => d.SubCategoryMain).WithMany(p => p.TableSubCategories)
                .HasForeignKey(d => d.SubCategoryMainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableSubCategory_TableCategory");
        });

        modelBuilder.Entity<TableTopic>(entity =>
        {
            entity.HasKey(e => e.TopicId);

            entity.ToTable("TableTopic");

            entity.Property(e => e.TopicId).HasColumnName("TopicID");
            entity.Property(e => e.TopicActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.TopicDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TopicDislikes).HasDefaultValueSql("((0))");
            entity.Property(e => e.TopicLikes).HasDefaultValueSql("((0))");
            entity.Property(e => e.TopicSharerId).HasColumnName("TopicSharerID");
            entity.Property(e => e.TopicSharerIp)
                .HasMaxLength(15)
                .HasColumnName("TopicSharerIP");
            entity.Property(e => e.TopicTitle).HasMaxLength(100);

            entity.HasOne(d => d.TopicCategoryNavigation).WithMany(p => p.TableTopics)
                .HasForeignKey(d => d.TopicCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TableTopic_TableCategory");

            entity.HasOne(d => d.TopicSharer).WithMany(p => p.TableTopics)
                .HasForeignKey(d => d.TopicSharerId)
                .HasConstraintName("FK_TableTopic_TableUser");

            entity.HasOne(d => d.TopicSubCategoryNavigation).WithMany(p => p.TableTopics)
                .HasForeignKey(d => d.TopicSubCategory)
                .HasConstraintName("FK_TableTopic_TableSubCategory");
        });

        modelBuilder.Entity<TableUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("TableUser");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.UserBio).HasMaxLength(200);
            entity.Property(e => e.UserCreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserIpAdress).HasMaxLength(15);
            entity.Property(e => e.UserMail).HasMaxLength(40);
            entity.Property(e => e.UserName).HasMaxLength(40);
            entity.Property(e => e.UserPassword).HasMaxLength(64);
            entity.Property(e => e.UserPp).HasColumnName("UserPP");
            entity.Property(e => e.UserWarning).HasDefaultValueSql("((3))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
