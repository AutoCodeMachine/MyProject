using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyProject.ViewModels;

namespace MyProject.Models;

public partial class MyProjectContext : DbContext
{
    public MyProjectContext(DbContextOptions<MyProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrator { get; set; }

    public virtual DbSet<Forum> Forum { get; set; }

    public virtual DbSet<ForumComments> ForumComments { get; set; }

    public virtual DbSet<ForumHikingGroup> ForumHikingGroup { get; set; }

    public virtual DbSet<ForumImage> ForumImage { get; set; }

    public virtual DbSet<ForumName> ForumName { get; set; }

    public virtual DbSet<HikingRecords> HikingRecords { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    public virtual DbSet<MemberPassword> MemberPassword { get; set; }

    public virtual DbSet<RouteDetail> RouteDetail { get; set; }

    public virtual DbSet<RouteInformation> RouteInformation { get; set; }

    public virtual DbSet<RouteLandmarkType> RouteLandmarkType { get; set; }

    public virtual DbSet<RouteLandmarks> RouteLandmarks { get; set; }

    public virtual DbSet<RouteLevel> RouteLevel { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.AdminNo).HasName("PK__Administ__719EC827DECDD7AC");

            entity.Property(e => e.AdminNo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AdminID).HasMaxLength(20);
            entity.Property(e => e.AdminPassword).HasMaxLength(20);
        });

        modelBuilder.Entity<Forum>(entity =>
        {
            entity.HasKey(e => e.PostID).HasName("PK__Forum__AA1260381094F41F");

            entity.Property(e => e.PostID)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ForumID)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MemberID)
                .HasMaxLength(36)
                .IsFixedLength();
            entity.Property(e => e.PostTime).HasColumnType("datetime");
            entity.Property(e => e.PostTitle).HasMaxLength(30);

            entity.HasOne(d => d.ForumName).WithMany(p => p.Forum)
                .HasForeignKey(d => d.ForumID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Forum__FourmID__52593CB8");

            entity.HasOne(d => d.Member).WithMany(p => p.Forum)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Forum__MemberID__625A9A57");
        });

        modelBuilder.Entity<ForumComments>(entity =>
        {
            entity.HasKey(e => e.CommentID).HasName("PK__ForumCom__C3B4DFAAC8DCA08A");

            entity.Property(e => e.CommentID)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ForumID)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MemberID)
                .HasMaxLength(36)
                .IsFixedLength();
            entity.Property(e => e.PostContents).HasMaxLength(255);
            entity.Property(e => e.PostID)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PostTime).HasColumnType("datetime");

            entity.HasOne(d => d.Forum).WithMany(p => p.ForumComments)
                .HasForeignKey(d => d.ForumID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ForumComm__Fourm__571DF1D5");

            entity.HasOne(d => d.Member).WithMany(p => p.ForumComments)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ForumComm__Membe__6442E2C9");

            entity.HasOne(d => d.Post).WithMany(p => p.ForumComments)
                .HasForeignKey(d => d.PostID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ForumComm__PostI__5629CD9C");
        });

        modelBuilder.Entity<ForumHikingGroup>(entity =>
        {
            entity.HasKey(e => e.PostID).HasName("PK__ForumHik__AA1260385B4D1FB1");

            entity.Property(e => e.PostID)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");
            entity.Property(e => e.ForumID)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.JoinMembers).HasMaxLength(1200);
            entity.Property(e => e.MemberID)
                .HasMaxLength(36)
                .IsFixedLength();
            entity.Property(e => e.PostTime).HasColumnType("datetime");
            entity.Property(e => e.PostTitle).HasMaxLength(30);

            entity.HasOne(d => d.Forum).WithMany(p => p.ForumHikingGroup)
                .HasForeignKey(d => d.ForumID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ForumHiki__Fourm__5AEE82B9");

            entity.HasOne(d => d.Member).WithMany(p => p.ForumHikingGroup)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ForumHiki__Membe__634EBE90");
        });

        modelBuilder.Entity<ForumImage>(entity =>
        {
            entity.HasKey(e => e.Image).HasName("PK__ForumIma__3294EFD467EA5489");

            entity.Property(e => e.Image)
                .HasMaxLength(18)
                .IsFixedLength();
            entity.Property(e => e.ImageType)
                .HasMaxLength(4)
                .IsFixedLength();
            entity.Property(e => e.PostID)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Post).WithMany(p => p.ForumImage)
                .HasForeignKey(d => d.PostID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ForumImag__PostI__756D6ECB");
        });

        modelBuilder.Entity<ForumName>(entity =>
        {
            entity.HasKey(e => e.ForumID).HasName("PK__ForumNam__96ABBBA51E7FAE4A");

            entity.Property(e => e.ForumID)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ForumName1)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("ForumName");
        });

        modelBuilder.Entity<HikingRecords>(entity =>
        {
            entity.HasKey(e => e.RecordID).HasName("PK__HikingRe__FBDF78C985920B60");

            entity.Property(e => e.RecordID)
                .HasMaxLength(40)
                .IsFixedLength();
            entity.Property(e => e.ImageName).HasMaxLength(46);
            entity.Property(e => e.LandmarkID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MemberID)
                .HasMaxLength(36)
                .IsFixedLength();
            entity.Property(e => e.UploadDate).HasColumnType("datetime");

            entity.HasOne(d => d.Landmark).WithMany(p => p.HikingRecords)
                .HasForeignKey(d => d.LandmarkID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HikingRec__Landm__5FB337D6");

            entity.HasOne(d => d.Member).WithMany(p => p.HikingRecords)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HikingRec__Membe__503BEA1C");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberID).HasName("PK__Member__0CF04B389D74D7C0");

            entity.Property(e => e.MemberID)
                .HasMaxLength(36)
                .IsFixedLength();
            entity.Property(e => e.AccountName).HasMaxLength(24);
            entity.Property(e => e.Email).HasMaxLength(40);
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.PersonalID)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.PersonalName).HasMaxLength(24);
        });

        modelBuilder.Entity<MemberPassword>(entity =>
        {
            entity.HasKey(e => e.MemberID).HasName("PK__MemberPa__0CF04B389AE55C4F");

            entity.Property(e => e.MemberID)
                .HasMaxLength(36)
                .IsFixedLength();
            entity.Property(e => e.AccountPassword).HasMaxLength(64);

            entity.HasOne(d => d.Member).WithOne(p => p.MemberPassword)
                .HasForeignKey<MemberPassword>(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberPas__Membe__4BAC3F29");
        });

        modelBuilder.Entity<RouteDetail>(entity =>
        {
            entity.HasKey(e => new { e.RouteID, e.LandmarkID }).HasName("PK__RouteDet__638EC1FABEE25361");

            entity.Property(e => e.RouteID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LandmarkID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RouteLevel)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TypeID)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Landmark).WithMany(p => p.RouteDetail)
                .HasForeignKey(d => d.LandmarkID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RouteDeta__Landm__4222D4EF");

            entity.HasOne(d => d.Route).WithMany(p => p.RouteDetail)
                .HasForeignKey(d => d.RouteID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RouteDeta__Route__403A8C7D");

            entity.HasOne(d => d.RouteLevelID).WithMany(p => p.RouteDetail)
                .HasForeignKey(d => d.RouteLevel)
                .HasConstraintName("FK__RouteDeta__Route__4316F928");

            entity.HasOne(d => d.Type).WithMany(p => p.RouteDetail)
                .HasForeignKey(d => d.TypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RouteDeta__TypeI__412EB0B6");
        });

        modelBuilder.Entity<RouteInformation>(entity =>
        {
            entity.HasKey(e => e.RouteID).HasName("PK__RouteInf__80979AAD16F25EBB");

            entity.Property(e => e.RouteID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RouteName).HasMaxLength(24);
        });

        modelBuilder.Entity<RouteLandmarkType>(entity =>
        {
            entity.HasKey(e => e.TypeID).HasName("PK__RouteLan__516F0395986144BF");

            entity.Property(e => e.TypeID)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TypeName).HasMaxLength(5);
        });

        modelBuilder.Entity<RouteLandmarks>(entity =>
        {
            entity.HasKey(e => e.LandmarkID).HasName("PK__RouteLan__3195B57F9B94BAD1");

            entity.Property(e => e.LandmarkID)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Distance).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.LandmarkName).HasMaxLength(20);
            entity.Property(e => e.NickName).HasMaxLength(50);
            entity.Property(e => e.TypeID)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.WaterSource).HasMaxLength(20);

            entity.HasOne(d => d.Type).WithMany(p => p.RouteLandmarks)
                .HasForeignKey(d => d.TypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RouteLand__TypeI__3B75D760");
        });

        modelBuilder.Entity<RouteLevel>(entity =>
        {
            entity.HasKey(e => e.RouteLevel1).HasName("PK__RouteLev__4BAF4B86D116F89C");

            entity.Property(e => e.RouteLevel1)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RouteLevel");
            entity.Property(e => e.HikingSuggestion).HasMaxLength(50);
            entity.Property(e => e.LevelDescription).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<MyProject.ViewModels.VMForum> VMForum { get; set; } = default!;
}
