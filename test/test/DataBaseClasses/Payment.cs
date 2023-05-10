using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DataBaseClasses
{
    public class Payment
    {
        public int Id { get; set; }
        public string SurName { get; set; }
        public string Name { get; set; }
        public int RoomNumber { get; set; }
        public int Cost { get; set; }
        public string isPaid { get; set; }
    }
}
