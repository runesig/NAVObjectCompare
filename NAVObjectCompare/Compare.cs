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

        private Dictionary<string, NavObject> _navObjectsA = null;
        private Dictionary<string, NavObject> _navObjectsB = null;

        private Dictionary<string, ObjectsCompared> _objectsComparedDict = new Dictionary<string, ObjectsCompared>();

        public Compare(string compareFileA, string compareFileB)
        {
            _compareFileA = compareFileA;
            _compareFileB = compareFileB;
        }

        public Dictionary<string, NavObject> NavObjectsA { get { return _navObjectsA; } }
        public Dictionary<string, NavObject> NavObjectsB { get { return _navObjectsB; } }

        public void RunCompare()
        {
            ObjectFile fileA = new ObjectFile(_compareFileA);
            _navObjectsA = fileA.Run();

            ObjectFile fileB = new ObjectFile(_compareFileB);
            _navObjectsB = fileB.Run();

            FindDifferencesA();
            FindDifferencesB();
        }

        public List<ObjectsCompared> GetList()
        {
            return _objectsComparedDict.Values.ToList<ObjectsCompared>();
        }

        private void FindDifferencesA()
        {
            foreach (string key in _navObjectsA.Keys)
            {
                NavObject navObjectA = GetDictValue(_navObjectsA, key);
                NavObject navObjectB = GetDictValue(_navObjectsB, key);

                ObjectsCompared objectsCompared = new ObjectsCompared(navObjectA.InternalId);
                objectsCompared.Id = navObjectA.Id;
                objectsCompared.Type = navObjectA.Type;
                objectsCompared.Name = navObjectA.Name;

                GetDifference(navObjectA, navObjectB, objectsCompared);

                SetAValues(navObjectA, objectsCompared);
                SetBValues(navObjectB, objectsCompared);

                if (!_objectsComparedDict.ContainsKey(objectsCompared.InternalId))
                    _objectsComparedDict.Add(objectsCompared.InternalId, objectsCompared);
            }
        }



        private void FindDifferencesB()
        {
            foreach (string key in _navObjectsB.Keys)
            {
                NavObject navObjectB = GetDictValue(_navObjectsB, key);
                NavObject navObjectA = GetDictValue(_navObjectsA, key);

                ObjectsCompared objectsCompared = new ObjectsCompared(navObjectB.InternalId);
                objectsCompared.Id = navObjectB.Id;
                objectsCompared.Type = navObjectB.Type;
                objectsCompared.Name = navObjectB.Name;

                GetDifference(navObjectB, navObjectA, objectsCompared);

                SetAValues(navObjectA, objectsCompared);
                SetBValues(navObjectB, objectsCompared);

                if (!_objectsComparedDict.ContainsKey(objectsCompared.InternalId))
                    _objectsComparedDict.Add(objectsCompared.InternalId, objectsCompared);
            }
        }

        private void GetDifference(NavObject navObject1, NavObject navObject2, ObjectsCompared objectsCompared)
        {
            if (!navObject1.IsEqualTo(navObject2))
            {
                objectsCompared.Equal = false;

                // Do More Analysis
                if (!navObject1.IsObjectPropertiesEqual(navObject2))
                {
                    objectsCompared.ObjectPropertiesEqual = false;
                    objectsCompared.Difference = "Date or Time";
                }

                if (!navObject1.IsPropertiesEqual(navObject2))
                {
                    objectsCompared.PropertiesEqual = false;
                    objectsCompared.Difference = objectsCompared.Difference += ", Properties";
                }

                if (!navObject1.IsCodeEqual(navObject2))
                {
                    objectsCompared.CodeEqual = false;
                    objectsCompared.Difference = objectsCompared.Difference  += ", Code";
                }
            }
            else
            {
                objectsCompared.Equal = true;
            }
        }

        private static void SetAValues(NavObject navObjectA, ObjectsCompared objectsCompared)
        {
            if (navObjectA != null)
            {
                objectsCompared.StringDateA = navObjectA.StringDate;
                objectsCompared.StringTimeA = navObjectA.StringTime;
                objectsCompared.VersionListA = navObjectA.VersionList;
                objectsCompared.NoOfLinesA = navObjectA.ObjectLines.Count;
            }
        }

        private static void SetBValues(NavObject navObjectB, ObjectsCompared objectsCompared)
        {
            if (navObjectB != null)
            {
                objectsCompared.StringDateB = navObjectB.StringDate;
                objectsCompared.StringTimeB = navObjectB.StringTime;
                objectsCompared.VersionListB = navObjectB.VersionList;
                objectsCompared.NoOfLinesB = navObjectB.ObjectLines.Count;
            }
        }

        private NavObject GetDictValue(Dictionary<string, NavObject> dict, string key)
        {
            NavObject navObject = null;
            if (!dict.TryGetValue(key, out navObject))
                return null;

            return navObject;
        }
    }
}
