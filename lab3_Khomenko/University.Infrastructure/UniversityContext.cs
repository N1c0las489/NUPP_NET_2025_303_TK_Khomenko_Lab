using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using University.Infrastructure.Models;

namespace University.Infrastructure
{
    public class UniversityContextFactory : IDesignTimeDbContextFactory<UniversityContext>
    {
        public UniversityContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UniversityContext>();
            optionsBuilder.UseSqlite("Data Source=university.db");

            return new UniversityContext(optionsBuilder.Options);
        }
    }

    public class UniversityContext : DbContext
    {
        public UniversityContext() { }

        public UniversityContext(DbContextOptions<UniversityContext> options)
            : base(options) { }

        public DbSet<StudentModel> Students { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<EnrollmentModel> Enrollments { get; set; }
        public DbSet<StudentAddressModel> StudentAddresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=university.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<StudentModel>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<StudentModel>()
                .Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<StudentModel>()
                .Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<StudentModel>()
                .HasOne(s => s.Address)
                .WithOne(a => a.Student)
                .HasForeignKey<StudentAddressModel>(a => a.StudentId);

            modelBuilder.Entity<CourseModel>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CourseModel>()
                .Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<EnrollmentModel>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<EnrollmentModel>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<EnrollmentModel>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<StudentAddressModel>()
                .HasKey(sa => sa.Id);

            modelBuilder.Entity<CourseModel>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<EnrollmentModel>()
               .Property(s => s.Id)
               .ValueGeneratedOnAdd();
            modelBuilder.Entity<StudentAddressModel>()
               .Property(s => s.Id)
               .ValueGeneratedOnAdd();
            modelBuilder.Entity<StudentModel>()
               .Property(s => s.Id)
               .ValueGeneratedOnAdd();
        }
    }
}