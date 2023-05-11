using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DataBaseClasses
{
    public class Revenue
    {
        public int Id { get; set; }
        public int DormitoryID { get; set; }
        public int RoomNumber { get; set; }
        public int RoomCost { get; set; }
        public int StudentID { get; set; }
        public string isPaid { get; set; }
    }
}
