﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Dtos
{
    public class AblityCreateDto
    {
        [Required(ErrorMessage = "必须要有Key")]
        public string AblityKey { get; set; }
        [Required(ErrorMessage = "必须要有Value")]
        public string AblityContent { get; set; }
    }
}
