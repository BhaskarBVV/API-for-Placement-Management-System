using Microsoft.EntityFrameworkCore;
using System;
using PMS_api.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS_api.Data
{
    public class ApiDbContext:DbContext
    {
        public DbSet<Student> Student { get; set; }
        public DbSet<Companies> Companies { get; set; }
        public DbSet<Placed> Placed { get; set; }
        public DbSet<AllowedStudents> AllowedStudents { get; set; } 
        public DbSet<User> User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("Connection"));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AllowedStudents>().HasKey(x => new { x.student_roll_no, x.company_id});
            builder.Entity<User>().HasData(new
            {
                userName = "admin",
                pass = "1234"
            });
        }
    }
}
