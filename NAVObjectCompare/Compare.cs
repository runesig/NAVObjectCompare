using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NAVObjectCompare
{
    public class Compare
    {
        private enum ObjectPart { Empty, NewObject, ObjectProperties, Properties, Code };

        private string _compareFileA = string.Empty;
        private string _compareFileB = string.Empty;

        private Dictionary<string, NavObject> _navObjectsA = new Dictionary<string, NavObject>();
        private Dictionary<string, NavObject> _navObjectsB = new Dictionary<string, NavObject>();

        public Compare(string compareFileA, string compareFileB)
        {
            _compareFileA = compareFileA;
            _compareFileB = compareFileB;
        }

        public void RunCompare()
        {
            ObjectFile fileA = new ObjectFile(_compareFileA);
            _navObjectsA = fileA.Run();

            ObjectFile fileB = new ObjectFile(_compareFileB);
            _navObjectsB = fileB.Run();

            FindDifferences();
        }

        private void FindDifferences()
        {
            foreach (string key in _navObjectsA.Keys)
            {
                NavObject navObjectA = _navObjectsA[key];
                NavObject navObjectB = _navObjectsB[key];

                if (!navObjectA.Equals(navObjectB))
                {
                    string test = "Test";
                    string test2 = test;
                }                   
            }
        }
    }
}
