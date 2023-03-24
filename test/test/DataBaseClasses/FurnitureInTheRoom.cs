using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DataBaseClasses
{
    public class FurnitureInTheRoom
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("HardInventory")]
        public int Id_Hard_Inventory { get; set; }
        public HardInventory HardInventory { get; set; }
        [ForeignKey("Room")]
        public int Id_room { get; set; }
        public Room Room { get; set; }
    }
}
