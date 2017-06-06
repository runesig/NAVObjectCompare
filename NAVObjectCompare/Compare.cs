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

        private List<NAVObject> _navObjectsA = new List<NAVObject>();
        private List<NAVObject> _navObjectsB = new List<NAVObject>();

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
        }

        public void FindDifferences()
        {
            foreach(NAVObject navObjectA in _navObjectsA)
            {
                foreach (NAVObject navObjectB in _navObjectsB)
                {
                    //if()
                    //navObjectA.Id
                }
            }
        }
    }
}
