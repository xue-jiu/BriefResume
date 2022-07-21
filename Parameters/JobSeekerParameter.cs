using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BriefResume.Parameters
{
    public class JobSeekerParameter
    {
        public string OrderBy { get; set; }
        public string Colleage { get; set; }
        public int? MaxExpadSalary { get; set; }
        public int? MinExpadSalary { get; set; }
        public string  Fields { get; set; }

    }
}
