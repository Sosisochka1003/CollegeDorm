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
            public DbSet<SoftInventory> SoftInventory { get; set; }
            public DbSet<Document> Document { get; set; }
            public DbSet<Speciality> Speciality { get; set; }
            public DbSet<Group> Group { get; set; }
            public DbSet<Parents> Parents { get; set; }
            public DbSet<StudentSoftStock> StudentSoftStock { get; set; }
            public DbSet<ListOfDocument> ListOfDocument { get; set; }
            public DbSet<Room> Room { get; set; }
            public DbSet<Student> Student { get; set; }
            public DbSet<FurnitureInTheRoom> FurnitureInTheRoom { get; set; }
            public DbSet<HardInventory> HardInventory { get; set; }
            
            public DormContext()
            {
                Database.EnsureCreated();
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=123");
            }


            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                
                //modelBuilder.Entity<Student>()
                //    .HasOne(s => s.Room)
                //    .WithMany(r => r.Students)
                //    .HasForeignKey(s => s.RoomId)
                //    .OnDelete(DeleteBehavior.Cascade)
                //    .HasForeignKey(s => s.Id_group)
                //    .HasForeignKey(s => s.Id_parents);

            }
        }

        public static void AddStudent(string name, int roomId)
        {
            using (var context = new DormContext())
            {
                var room = context.Room.FirstOrDefault(r => r.Id == roomId);
                if (room == null)
                {
                    MessageBox.Show($"Room {roomId} not found.");
                    return;
                }

                var student = new Student
                {
                    Name = name,
                    RoomId = roomId
                };

                context.Student.Add(student);
                context.SaveChanges();
            }
        }

        public static void AddRoom(int roomNumber, decimal cost)
        {
            using (var context = new DormContext())
            {
                var room = new Room
                {
                    RoomNumber = roomNumber,
                    Cost = cost
                };

                context.Room.Add(room);
                context.SaveChanges();
            }
        }

        public static void ListStudents()
        {
            using (var context = new DormContext())
            {
                var students = context.Student.Include(s => s.Room).ToList();
                MessageBox.Show("Students:");
                foreach (var student in students)
                {
                    MessageBox.Show($"{student.Name} - Room {student.Room.RoomNumber}");
                }
            }
        }

        public static void ListRoom()
        {
            using (var context = new DormContext())
            {
                var Room = context.Room.ToList();
                MessageBox.Show("Room:");
                foreach (var room in Room)
                {
                    MessageBox.Show($"Room {room.RoomNumber} - Cost: {room.Cost:C}");
                }
            }
        }

        public static void ListStudentsInRoom(int roomId)
        {
            using (var context = new DormContext())
            {
                var room = context.Room.Include(r => r.Students).FirstOrDefault(r => r.Id == roomId);
                if (room == null)
                {
                    MessageBox.Show($"Room {roomId} not found.");
                    return;
                }

                MessageBox.Show($"Students in Room {room.RoomNumber}:");
                foreach (var student in room.Students)
                {
                    MessageBox.Show(student.Name);
                }
            }
        }

        public static void GetRoomCost(int roomId)
        {
            using (var context = new DormContext())
            {
                var room = context.Room.FirstOrDefault(r => r.Id == roomId);
                if (room == null)
                {
                    MessageBox.Show($"Room {roomId} not found.");
                    return;
                }
                MessageBox.Show($"This room {roomId} is worth {room.Cost}");
            }
        }
    }
}
