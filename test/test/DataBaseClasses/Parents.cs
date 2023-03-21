using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace test.DataBaseClasses
{
    public class Parents
    {
        [Key]
        public int Id { get; set; }
        public string Mother { get; set; }
        public string Father { get; set; }
        public bool Marriage { get; set; }
    }
}
