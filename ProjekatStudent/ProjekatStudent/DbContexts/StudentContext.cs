using Microsoft.EntityFrameworkCore;
using ProjekatStudent.Entities;

namespace ProjekatStudent.DbContexts
{
    public class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Join> Joins { get; set; }
        public DbSet<User> Users { get; set; }

        public StudentContext(DbContextOptions options) : base(options)
        {
               
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Join>()
                .HasKey(j => new { j.StudentId, j.ExamId });

            modelBuilder.Entity<Join>()
                .HasOne(j => j.Student)
                .WithMany(s => s.PassedExams)
                .HasForeignKey(k => k.StudentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Join>()
                .HasOne(j => j.Exam)
                .WithMany(e => e.ListOfPassedExams)
                .HasForeignKey(k => k.ExamId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
