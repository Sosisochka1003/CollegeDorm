using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DataBaseClasses
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public decimal Cost { get; set; }
        [ForeignKey("Dormitory")]
        public int DormitoryId { get; set; }
        public Dormitory Dormitory { get; set; }
        public int Living_space { get; set; }
        public int Number_of_beds { get; set; }
        public List<Student> Students { get; set; }
    }
}
