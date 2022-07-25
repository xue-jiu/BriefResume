using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Helper
{
    public class Linker
    {
        public string Href { get; set; }
        public string Relation { get; }
        public string Method { get; set; }

        public Linker(string href,string relation, string method)
        {
            Method = method;
            Href = href;
            Relation = relation;
        }
    }
}
