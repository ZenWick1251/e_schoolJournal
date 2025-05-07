using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using e_journal.Models;


namespace e_journal.Data;


    public class JournalDbContext : DbContext
{
    public JournalDbContext(DbContextOptions<JournalDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<SchoolClass> SchoolClasses { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<LessonSchedule> LessonSchedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<Student>()
            .HasOne(s => s.User)
            .WithOne(u => u.Student)
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.Entity<Teacher>()
            .HasOne(t => t.User)
            .WithOne(u => u.Teacher)
            .HasForeignKey<Teacher>(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.Entity<SchoolClass>()
            .HasMany(c => c.Students)
            .WithOne(s => s.SchoolClass)
            .HasForeignKey(s => s.SchoolClassId);

        
        modelBuilder.Entity<Subject>()
            .HasMany(s => s.Grades)
            .WithOne(g => g.Subject)
            .HasForeignKey(g => g.SubjectId)
            .OnDelete(DeleteBehavior.Restrict); 


        
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Grades)
            .WithOne(g => g.Student)
            .HasForeignKey(g => g.StudentId);

        
        modelBuilder.Entity<Teacher>()
            .HasMany(t => t.Subjects)
            .WithOne(s => s.Teacher)
            .HasForeignKey(s => s.TeacherId);

        
        modelBuilder.Entity<LessonSchedule>()
            .HasOne(ls => ls.SchoolClass)
            .WithMany()
            .HasForeignKey(ls => ls.SchoolClassId);

        modelBuilder.Entity<LessonSchedule>()
            .HasOne(ls => ls.Subject)
            .WithMany()
            .HasForeignKey(ls => ls.SubjectId);
    }
}