using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AwoAppServices.Models
{
    public partial class GymadminContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public GymadminContext()
        {
        }

        public GymadminContext(DbContextOptions<GymadminContext> options)
            : base(options)
        { 
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Exercises> Exercises { get; set; }
        public virtual DbSet<GymUsers> GymUsers { get; set; }
        public virtual DbSet<Notes> Notes { get; set; }
        public virtual DbSet<Objectives> Objectives { get; set; }
        public virtual DbSet<UserExercises> UserExercises { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=MSI; Database=Gymadmin ;Trusted_Connection=True;");
            }
        }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Exercises>(entity =>
            {
                entity.HasKey(e => e.ExerciseId);

                entity.HasIndex(e => e.CategoryId);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exercises_Categories");
            });

            modelBuilder.Entity<GymUsers>(entity =>
            {
                entity.HasKey(e => e.GymUserId)
                    .HasName("PK_Users");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    //.IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    //.IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Telephone)
                    //.IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Notes>(entity =>
            {
                entity.HasKey(e => e.NoteId);

                entity.Property(e => e.Header)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Objectives>(entity =>
            {
                entity.HasKey(e => e.GymObjectiveId);

                entity.HasIndex(e => e.ExerciseId);

                entity.HasIndex(e => e.GymUserId)
                    .HasName("IX_Objectives_UserId");

                entity.Property(e => e.OjectiveInfo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.Objectives)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Objectives_Exercises");

                entity.HasOne(d => d.GymUser)
                    .WithMany(p => p.Objectives)
                    .HasForeignKey(d => d.GymUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Objectives_Users");
            });

            modelBuilder.Entity<UserExercises>(entity =>
            {
                entity.HasKey(e => e.UserExerciseId);

                entity.HasIndex(e => e.ExerciseId);

                entity.HasIndex(e => e.GymUserId)
                    .HasName("IX_UserExercises_UserId");

                entity.Property(e => e.ExerciseDate).HasColumnType("datetime");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.UserExercises)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserExercises_Exercises");

                entity.HasOne(d => d.GymUser)
                    .WithMany(p => p.UserExercises)
                    .HasForeignKey(d => d.GymUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserExercises_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
