using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Dtos
{
    public class SeekerDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateDatetime { get; set; }
        public string Preference { get; set; }

        public InterviewerAttributeDto InterviewerAttribute { get; set; }
        public SeekerAttributeDto SeekerAttribute { get; set; }
    }
}
