using System;
using System.Collections.Generic;
using Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Data.DataContext;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID =postgres;Password=deep2292002;Server=localhost;Port=5432;Database=Assignment;Integrated Security=true;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Course_pkey");

            entity.ToTable("Course");

            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Student_pkey");

            entity.ToTable("Student");

            entity.Property(e => e.Course).HasColumnType("character varying");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email).HasColumnType("character varying");
            entity.Property(e => e.FirstName).HasColumnType("character varying");
            entity.Property(e => e.Gender).HasColumnType("character varying");
            entity.Property(e => e.Grade).HasColumnType("character varying");
            entity.Property(e => e.LastName).HasColumnType("character varying");

            entity.HasOne(d => d.CourseNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("courseid_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
