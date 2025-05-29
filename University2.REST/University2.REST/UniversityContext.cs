using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using University2.REST.Models;

namespace University2.REST
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

    public class UniversityContext : IdentityDbContext<ApplicationUser>
    {
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
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<CourseModel>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CourseModel>()
                .Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<EnrollmentModel>()
                .HasKey(e => e.Id);

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