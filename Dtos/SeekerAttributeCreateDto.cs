﻿using BriefResume.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Dtos
{
    public class SeekerAttributeCreateDto
    {
        public int ExpadSalary { get; set; }
        [MaxLength(50)]
        public string Colleage { get; set; }
        public string Degree { get; set; }
    }
}
