using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;

namespace test.DataBaseClasses
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string DName { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public string StudentSurname { get; set; }
        public Student Student { get; set; }

    }
}
