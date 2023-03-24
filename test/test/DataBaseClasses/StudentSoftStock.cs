using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DataBaseClasses
{
    public class StudentSoftStock
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student student { get; set; }
        [ForeignKey("SoftInventory")]
        public int SoftInventoryId { get; set; }
        public SoftInventory SoftInventory { get; set; }
        public DateTime Date_issue { get; set; }
    }
}
