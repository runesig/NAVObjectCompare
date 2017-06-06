using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompare
{
    public class NAVObject
    {
        public List<string> _lines = null;

        public NAVObject()
        {
            _lines = new List<string>();
        }

        public string Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public bool Modified { get; set; } 
        public string VersionList { get; set; }

        public void AddLine(string line)
        {
            _lines.Add(line);
        }
    }
}
