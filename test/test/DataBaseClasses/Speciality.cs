﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DataBaseClasses
{
    public class Speciality
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
