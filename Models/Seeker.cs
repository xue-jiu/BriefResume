using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Models
{
    public class Seeker : IdentityUser<Guid>
    {
        public string Preference { get; set; }
        public InterviewerAttribute InterviewerAttribute { get; set; }
        public SeekerAttribute SeekerAttribute { get; set; }
        public DateTime CreateDatetime { get; set; }
    }
}
