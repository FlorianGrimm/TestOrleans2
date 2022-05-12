using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Replacement.OData
{
    public partial class TodoDBContext : DbContext
    {
        public TodoDBContext()
        {
        }

        public TodoDBContext(DbContextOptions<TodoDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Operation> Operation { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectHistory> ProjectHistory { get; set; }
        public virtual DbSet<ToDo> ToDo { get; set; }
        public virtual DbSet<ToDoHistory> ToDoHistory { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserHistory> UserHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=Database");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var longToBytesConverter = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.NumberToBytesConverter<long>();

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.HasKey(e => new { e.CreatedAt, e.OperationId })
                    .HasName("PK_dbo_Operation");

                entity.Property(e => e.EntityId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EntityType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SerialVersion)
                    .IsRequired()
                    .HasConversion(longToBytesConverter)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    ;

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId).ValueGeneratedNever();

                entity.Property(e => e.SerialVersion)
                    .IsRequired()
                    .HasConversion(longToBytesConverter)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => new { d.ModifiedAt, d.OperationId })
                    .HasConstraintName("FK_Project_Operation");
            });

            modelBuilder.Entity<ProjectHistory>(entity =>
            {
                entity.HasKey(e => new { e.ValidTo, e.ValidFrom, e.OperationId, e.ProjectId })
                    .HasName("PK_history_ProjectHistory");

                entity.ToTable("ProjectHistory", "history");

                entity.Property(e => e.SerialVersion)
                    .IsRequired()
                    .HasConversion(longToBytesConverter)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.ProjectHistory)
                    .HasForeignKey(d => new { d.ValidFrom, d.OperationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_history_ProjectHistory_dbo_Operation");
            });

            modelBuilder.Entity<ToDo>(entity =>
            {
                entity.HasKey(e => new { e.ProjectId, e.ToDoId })
                    .HasName("PK_dbo_ToDo");

                entity.Property(e => e.SerialVersion)
                    .IsRequired()
                    .HasConversion(longToBytesConverter)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ToDo)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo_ToDo_dbo_Project");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ToDo)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo_ToDo_dbo_User");

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.ToDo)
                    .HasForeignKey(d => new { d.ModifiedAt, d.OperationId })
                    .HasConstraintName("FK_dbo_ToDo_dbo_Operation");
            });

            modelBuilder.Entity<ToDoHistory>(entity =>
            {
                entity.HasKey(e => new { e.ValidTo, e.ValidFrom, e.OperationId, e.ProjectId, e.ToDoId })
                    .HasName("PK_history_ToDoistory");

                entity.ToTable("ToDoHistory", "history");

                entity.Property(e => e.SerialVersion)
                    .IsRequired()
                    .HasConversion(longToBytesConverter)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.ToDoHistory)
                    .HasForeignKey(d => new { d.ValidFrom, d.OperationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_history_ToDoHistory_dbo_Operation");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName, "UX_dbo_User_UserName")
                    .IsUnique();

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.SerialVersion)
                    .IsRequired()
                    .HasConversion(longToBytesConverter)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => new { d.ModifiedAt, d.OperationId })
                    .HasConstraintName("FK_dbo_User_dbo_Operation");
            });

            modelBuilder.Entity<UserHistory>(entity =>
            {
                entity.HasKey(e => new { e.ValidTo, e.ValidFrom, e.OperationId, e.UserId })
                    .HasName("PK_history_UserHistory");

                entity.ToTable("UserHistory", "history");

                entity.Property(e => e.SerialVersion)
                    .IsRequired()
                    .HasConversion(longToBytesConverter)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Operation)
                    .WithMany(p => p.UserHistory)
                    .HasForeignKey(d => new { d.ValidFrom, d.OperationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_history_UserHistory_dbo_Operation");
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
