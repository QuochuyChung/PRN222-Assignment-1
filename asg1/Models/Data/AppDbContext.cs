using asg1.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace asg1.Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<RubricCriterion> RubricCriteria { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>()
                .HasIndex(s => s.Code)
                .IsUnique();

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Subject)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RubricCriterion>()
                .HasOne(r => r.Question)
                .WithMany(q => q.RubricCriteria)
                .HasForeignKey(r => r.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RubricCriterion>()
                .Property(r => r.MaxScore)
                .HasPrecision(5, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
