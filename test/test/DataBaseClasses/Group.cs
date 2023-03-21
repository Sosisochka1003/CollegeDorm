using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DataBaseClasses
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        public string Number { get; set; }
        [ForeignKey("Speciality")]
        public int Id_spec { get; set; }
        public Speciality Speciality { get; set; }
    }
}
