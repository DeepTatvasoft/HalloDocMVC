using System;
using System.Collections.Generic;
using HalloDoc.Models;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DataContext;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Adminregion> Adminregions { get; set; }

    public virtual DbSet<Aspnetrole> Aspnetroles { get; set; }

    public virtual DbSet<Aspnetuser> Aspnetusers { get; set; }

    public virtual DbSet<Aspnetuserrole> Aspnetuserroles { get; set; }

    public virtual DbSet<Blockrequest> Blockrequests { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Businesstype> Businesstypes { get; set; }

    public virtual DbSet<Casetag> Casetags { get; set; }

    public virtual DbSet<Concierge> Concierges { get; set; }

    public virtual DbSet<Emaillog> Emaillogs { get; set; }

    public virtual DbSet<Healthprofessional> Healthprofessionals { get; set; }

    public virtual DbSet<Healthprofessionaltype> Healthprofessionaltypes { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Physician> Physicians { get; set; }

    public virtual DbSet<Physicianlocation> Physicianlocations { get; set; }

    public virtual DbSet<Physiciannotification> Physiciannotifications { get; set; }

    public virtual DbSet<Physicianregion> Physicianregions { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Requestbusiness> Requestbusinesses { get; set; }

    public virtual DbSet<Requestclient> Requestclients { get; set; }

    public virtual DbSet<Requestclosed> Requestcloseds { get; set; }

    public virtual DbSet<Requestconcierge> Requestconcierges { get; set; }

    public virtual DbSet<Requestnote> Requestnotes { get; set; }

    public virtual DbSet<Requeststatuslog> Requeststatuslogs { get; set; }

    public virtual DbSet<Requesttype> Requesttypes { get; set; }

    public virtual DbSet<Requestwisefile> Requestwisefiles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rolemenu> Rolemenus { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Shiftdetail> Shiftdetails { get; set; }

    public virtual DbSet<Shiftdetailregion> Shiftdetailregions { get; set; }

    public virtual DbSet<Smslog> Smslogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID = postgres;Password=deep2292002;Server=localhost;Port=5432;Database=HalloDocMVC;Integrated Security=true;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Adminid).HasName("admin_pkey");

            entity.Property(e => e.Adminid).ValueGeneratedNever();

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.AdminAspnetusers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("aspnetuserid_fk");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.AdminCreatedbyNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CreatedBy_fk");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.AdminModifiedbyNavigations).HasConstraintName("ModifiedBy_fk");
        });

        modelBuilder.Entity<Adminregion>(entity =>
        {
            entity.HasKey(e => e.Adminregionid).HasName("adminregion_pkey");

            entity.Property(e => e.Adminregionid).ValueGeneratedNever();

            entity.HasOne(d => d.Admin).WithMany(p => p.Adminregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AdminId_fk");

            entity.HasOne(d => d.Region).WithMany(p => p.Adminregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RegionId_fk");
        });

        modelBuilder.Entity<Aspnetrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("aspnetroles_pkey");
        });

        modelBuilder.Entity<Aspnetuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("aspnetusers_pkey");
        });

        modelBuilder.Entity<Aspnetuserrole>(entity =>
        {
            entity.HasKey(e => new { e.Userid, e.Roleid }).HasName("aspnetuserroles_pkey");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserroles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserId_fk");
        });

        modelBuilder.Entity<Blockrequest>(entity =>
        {
            entity.HasKey(e => e.Blockrequestid).HasName("blockrequests_pkey");

            entity.Property(e => e.Blockrequestid).ValueGeneratedNever();
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.Businessid).HasName("business_pkey");

            entity.Property(e => e.Businessid).ValueGeneratedNever();

            entity.HasOne(d => d.Businesstype).WithMany(p => p.Businesses).HasConstraintName("BusinessTypeId_fk");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.BusinessCreatedbyNavigations).HasConstraintName("CreatedBy_fk");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.BusinessModifiedbyNavigations).HasConstraintName("ModifiedBy_fk");

            entity.HasOne(d => d.Region).WithMany(p => p.Businesses).HasConstraintName("RegionId_fk");
        });

        modelBuilder.Entity<Businesstype>(entity =>
        {
            entity.HasKey(e => e.Businesstypeid).HasName("businesstype_pkey");

            entity.Property(e => e.Businesstypeid).ValueGeneratedNever();
        });

        modelBuilder.Entity<Casetag>(entity =>
        {
            entity.HasKey(e => e.Casetagid).HasName("casetag_pkey");

            entity.Property(e => e.Casetagid).ValueGeneratedNever();
        });

        modelBuilder.Entity<Concierge>(entity =>
        {
            entity.HasKey(e => e.Conciergeid).HasName("concierge_pkey");

            entity.Property(e => e.Conciergeid).ValueGeneratedNever();

            entity.HasOne(d => d.Region).WithMany(p => p.Concierges)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RegionId_fk");
        });

        modelBuilder.Entity<Emaillog>(entity =>
        {
            entity.HasKey(e => e.Emaillogid).HasName("emaillog_pkey");
        });

        modelBuilder.Entity<Healthprofessional>(entity =>
        {
            entity.HasKey(e => e.Vendorid).HasName("healthprofessionals_pkey");

            entity.Property(e => e.Vendorid).ValueGeneratedNever();

            entity.HasOne(d => d.ProfessionNavigation).WithMany(p => p.Healthprofessionals).HasConstraintName("Profession_fk");
        });

        modelBuilder.Entity<Healthprofessionaltype>(entity =>
        {
            entity.HasKey(e => e.Healthprofessionalid).HasName("healthprofessionaltype_pkey");

            entity.Property(e => e.Healthprofessionalid).ValueGeneratedNever();
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Menuid).HasName("menu_pkey");

            entity.Property(e => e.Menuid).ValueGeneratedNever();
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderdetails_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Physician>(entity =>
        {
            entity.HasKey(e => e.Physicianid).HasName("physician_pkey");

            entity.Property(e => e.Physicianid).ValueGeneratedNever();

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.PhysicianAspnetusers).HasConstraintName("AspNetUserId_fk");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.PhysicianCreatedbyNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CreatedBy_fk");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.PhysicianModifiedbyNavigations).HasConstraintName("ModifiedBy_fk");
        });

        modelBuilder.Entity<Physiciannotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("physiciannotification_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Physician).WithMany(p => p.Physiciannotifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PhysicianId_fk");
        });

        modelBuilder.Entity<Physicianregion>(entity =>
        {
            entity.HasKey(e => e.Physicianregionid).HasName("physicianregion_pkey");

            entity.Property(e => e.Physicianregionid).ValueGeneratedNever();

            entity.HasOne(d => d.Physician).WithMany(p => p.Physicianregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PhysicianId_fk");

            entity.HasOne(d => d.Region).WithMany(p => p.Physicianregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RegionId_fk");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Regionid).HasName("region_pkey");

            entity.Property(e => e.Regionid).ValueGeneratedNever();
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Requestid).HasName("request_pkey");

            entity.Property(e => e.Requestid).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Requests).HasConstraintName("UserId_fk");
        });

        modelBuilder.Entity<Requestbusiness>(entity =>
        {
            entity.HasKey(e => e.Requestbusinessid).HasName("requestbusiness_pkey");

            entity.Property(e => e.Requestbusinessid).ValueGeneratedNever();

            entity.HasOne(d => d.Business).WithMany(p => p.Requestbusinesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BusinessId_fk");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestbusinesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequestId_fk");
        });

        modelBuilder.Entity<Requestclient>(entity =>
        {
            entity.HasKey(e => e.Requestclientid).HasName("requestclient_pkey");

            entity.Property(e => e.Requestclientid).ValueGeneratedNever();

            entity.HasOne(d => d.Region).WithMany(p => p.Requestclients).HasConstraintName("RegionId_fk");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestclients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequestId_fk");
        });

        modelBuilder.Entity<Requestclosed>(entity =>
        {
            entity.HasKey(e => e.Requestclosedid).HasName("requestclosed_pkey");

            entity.Property(e => e.Requestclosedid).ValueGeneratedNever();

            entity.HasOne(d => d.Request).WithMany(p => p.Requestcloseds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequestId_fk");

            entity.HasOne(d => d.Requeststatuslog).WithMany(p => p.Requestcloseds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequestStatusLogId_fk");
        });

        modelBuilder.Entity<Requestconcierge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("requestconcierge_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Concierge).WithMany(p => p.Requestconcierges)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ConciergeId_fk");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestconcierges)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequestId_fk");
        });

        modelBuilder.Entity<Requestnote>(entity =>
        {
            entity.HasKey(e => e.Requestnotesid).HasName("requestnotes_pkey");

            entity.Property(e => e.Requestnotesid).ValueGeneratedNever();

            entity.HasOne(d => d.Request).WithMany(p => p.Requestnotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequestId_fk");
        });

        modelBuilder.Entity<Requeststatuslog>(entity =>
        {
            entity.HasKey(e => e.Requeststatuslogid).HasName("requeststatuslog_pkey");

            entity.Property(e => e.Requeststatuslogid).ValueGeneratedNever();

            entity.HasOne(d => d.Admin).WithMany(p => p.Requeststatuslogs).HasConstraintName("AdminId_fk");

            entity.HasOne(d => d.Physician).WithMany(p => p.RequeststatuslogPhysicians).HasConstraintName("PhysicianId_fk");

            entity.HasOne(d => d.Request).WithMany(p => p.Requeststatuslogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequestId_fk");

            entity.HasOne(d => d.Transtophysician).WithMany(p => p.RequeststatuslogTranstophysicians).HasConstraintName("TransToPhysicianId_fk");
        });

        modelBuilder.Entity<Requesttype>(entity =>
        {
            entity.HasKey(e => e.Requesttypeid).HasName("requesttype_pkey");

            entity.Property(e => e.Requesttypeid).ValueGeneratedNever();
        });

        modelBuilder.Entity<Requestwisefile>(entity =>
        {
            entity.HasKey(e => e.Requestwisefileid).HasName("requestwisefile_pkey");

            entity.Property(e => e.Requestwisefileid).ValueGeneratedNever();

            entity.HasOne(d => d.Admin).WithMany(p => p.Requestwisefiles).HasConstraintName("AdminId_fk");

            entity.HasOne(d => d.Physician).WithMany(p => p.Requestwisefiles).HasConstraintName("PhysicianId_fk");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestwisefiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequestId_fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pkey");

            entity.Property(e => e.Roleid).ValueGeneratedNever();
        });

        modelBuilder.Entity<Rolemenu>(entity =>
        {
            entity.HasKey(e => e.Rolemenuid).HasName("rolemenu_pkey");

            entity.Property(e => e.Rolemenuid).ValueGeneratedNever();

            entity.HasOne(d => d.Menu).WithMany(p => p.Rolemenus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MenuId_fk");

            entity.HasOne(d => d.Role).WithMany(p => p.Rolemenus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RoleId_fk");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Shiftid).HasName("shift_pkey");

            entity.Property(e => e.Shiftid).ValueGeneratedNever();
            entity.Property(e => e.Weekdays).IsFixedLength();

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.Shifts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CreatedBy_fk");

            entity.HasOne(d => d.Physician).WithMany(p => p.Shifts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PhysicianId_fk");
        });

        modelBuilder.Entity<Shiftdetail>(entity =>
        {
            entity.HasKey(e => e.Shiftdetailid).HasName("shiftdetail_pkey");

            entity.Property(e => e.Shiftdetailid).ValueGeneratedNever();

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.Shiftdetails).HasConstraintName("ModifiedBy_fk");

            entity.HasOne(d => d.Shift).WithMany(p => p.Shiftdetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ShiftId_fk");
        });

        modelBuilder.Entity<Shiftdetailregion>(entity =>
        {
            entity.HasKey(e => e.Shiftdetailregionid).HasName("shiftdetailregion_pkey");

            entity.Property(e => e.Shiftdetailregionid).ValueGeneratedNever();

            entity.HasOne(d => d.Region).WithMany(p => p.Shiftdetailregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RegionId_fk");

            entity.HasOne(d => d.Shiftdetail).WithMany(p => p.Shiftdetailregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ShiftDetailId_fk");
        });

        modelBuilder.Entity<Smslog>(entity =>
        {
            entity.HasKey(e => e.Smslogid).HasName("smslog_pkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.Users).HasConstraintName("AspNetUserId_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
