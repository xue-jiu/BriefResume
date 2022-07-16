using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Models
{
    public class InterviewerAttribute
    {
        public Guid InterviewerAttributeId { get; set; }

        public string InterviewerId { get; set; }
        public Seeker seeker { get; set; }

        public string Company { get; set; }
        public int Remuneration { get; set; }
    }
}
