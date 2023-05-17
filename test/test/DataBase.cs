using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using test.DataBaseClasses;

namespace test
{
    internal class DataBase
    {
        public class DormContext : DbContext
        {
            public DbSet<Dormitory> Dormitory { get; set; }
            public DbSet<Document> Document { get; set; }
            public DbSet<Speciality> Speciality { get; set; }
            public DbSet<Group> Group { get; set; }
            public DbSet<Parents> Parents { get; set; }
            public DbSet<StudentSoftStock> StudentSoftStock { get; set; }
            public DbSet<Room> Room { get; set; }
            public DbSet<Student> Student { get; set; }
            public DbSet<HardInventoryRoom> HardInventoryRoom { get; set; }
            public DbSet<Payment> Paymentl { get; set; }
            public DbSet<Revenue> Revenues { get; set; }

            public DormContext()
            {
                Database.EnsureCreated();
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                 optionsBuilder.UseNpgsql($"Host={test.Settings1.Default.Host};Port={test.Settings1.Default.Port};Database={test.Settings1.Default.DataBase};Username={test.Settings1.Default.UserName};Password={test.Settings1.Default.Password}");
            }


            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
            }
        }
    }
}
