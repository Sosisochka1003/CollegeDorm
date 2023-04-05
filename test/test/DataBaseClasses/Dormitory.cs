using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace test.DataBaseClasses
{


    public class Dormitory
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; }
        public int Numbers_of_rooms { get; set; }
    }
}
