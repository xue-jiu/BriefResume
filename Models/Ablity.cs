using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Models
{
    public class Ablity
    {
        public Guid JobSeekerId { get; set; }
        public SeekerAttribute SeekerAttribute { get; set; }
        public string AblityKey { get; set; }
        public string AblityContent { get; set; }
    }
}
