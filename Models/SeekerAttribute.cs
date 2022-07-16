using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Models
{
    public class SeekerAttribute
    {
        public Guid SeekerAttributeId { get; set; }

        public string JobSeekerId { get; set; }
        public Seeker seeker { get; set; }

        public int ExpadSalary { get; set; }
        public string Colleage { get; set; }
        public degree Degree { get; set; }

        public List<Ablity> Ablities { get; set; } = new List<Ablity>();
    }
}
