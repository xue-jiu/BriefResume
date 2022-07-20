using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Dtos
{
    public class SeekerUpdateDto
    {
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string UserName { get; set; }//此处需要用正则表达式添加数据验证,对应Identity的默认username要求
        public string Preference { get; set; }
    }
}
