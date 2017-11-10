using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompare.Models
{
    public class NavObjectsCompared
    {
        public enum Staus { Equal = 0, Unequal = 1, Unexisting = 2 }

        public NavObjectsCompared(string internalId)
        {
            this.InternalId = internalId;
        }
        public bool Selected { get; set; }
        public string InternalId { get; private set; }
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string StringDateA { get; set; }
        public string StringDateB { get; set; }
        public string StringTimeA { get; set; }
        public string StringTimeB { get; set; }
        public string VersionListA { get; set; }
        public string VersionListB { get; set; }
        public int NoOfLinesA { get; set; }
        public int NoOfLinesB { get; set; }
        public Staus Status { get; set; }
        public bool ObjectPropertiesEqual { get; set; }
        public bool CodeEqual { get; set; }
        public string Comment { get; set; }
    }
}
