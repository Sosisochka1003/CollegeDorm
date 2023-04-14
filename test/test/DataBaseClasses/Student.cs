using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Media.Animation;
using static test.DataBase;

namespace test.DataBaseClasses
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Home_Address { get; set; }
        public bool Status_learning { get; set; }
        public string Form_of_education { get; set; }
        public string Status_residence { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public Group Group { get; set; }
        [ForeignKey("Parents")]
        public int ParentsId { get; set; }
        public Parents Parents { get; set; }
    }
}
