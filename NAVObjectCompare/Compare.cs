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
        public string FilenameA { get { return _compareFileA; } }
        public string FilenameB { get { return _compareFileB; } }

        public void FindDifferencesA()
        {
            foreach (string internalId in _navObjectsA.Keys)
            {
                FindDifferencesA(internalId);
            }
        }

        public void FindDifferencesA(string internalId)
        {
            NavObject navObjectA = ObjectHelper.GetDictValue(_navObjectsA, internalId);
            NavObject navObjectB = ObjectHelper.GetDictValue(_navObjectsB, internalId);

            ObjectsCompared objectsCompared = new ObjectsCompared(internalId);
            objectsCompared.Id = navObjectA.Id;
            objectsCompared.Type = navObjectA.Type;
            objectsCompared.Name = navObjectA.Name;

            GetDifference(navObjectA, navObjectB, objectsCompared);

            SetAValues(navObjectA, objectsCompared);
            SetBValues(navObjectB, objectsCompared);

            if (!_objectsComparedDict.ContainsKey(internalId))
                _objectsComparedDict.Add(internalId, objectsCompared);
            else
                _objectsComparedDict[internalId] = objectsCompared;
        }

        public void FindDifferencesB()
        {
            foreach (string internalId in _navObjectsB.Keys)
            {
                FindDifferencesB(internalId);
            }
        }

        public void FindDifferencesB(string internalId)
        {
            NavObject navObjectB = ObjectHelper.GetDictValue(_navObjectsB, internalId);
            NavObject navObjectA = ObjectHelper.GetDictValue(_navObjectsA, internalId);

            ObjectsCompared objectsCompared = new ObjectsCompared(internalId);
            objectsCompared.Id = navObjectB.Id;
            objectsCompared.Type = navObjectB.Type;
            objectsCompared.Name = navObjectB.Name;

            GetDifference(navObjectB, navObjectA, objectsCompared);

            SetAValues(navObjectA, objectsCompared);
            SetBValues(navObjectB, objectsCompared);

            if (!_objectsComparedDict.ContainsKey(internalId))
                _objectsComparedDict.Add(internalId, objectsCompared);
            else
                _objectsComparedDict[internalId] = objectsCompared;

        }

        private void GetDifference(NavObject navObject1, NavObject navObject2, ObjectsCompared objectsCompared)
        {
            objectsCompared.CodeEqual = true;
            objectsCompared.ObjectPropertiesEqual = true;
            objectsCompared.Equal = true;

            if (!navObject1.IsEqualTo(navObject2))
            {
                objectsCompared.Equal = false;

                // Do More Analysis
                if (!navObject1.IsObjectPropertiesEqual(navObject2))
                {
                    objectsCompared.ObjectPropertiesEqual = false;
                    objectsCompared.Difference = "Date or Time";
                }

                //if (!navObject1.IsPropertiesEqual(navObject2))
                //{
                //    objectsCompared.PropertiesEqual = false;
                //    if (!string.IsNullOrEmpty(objectsCompared.Difference))
                //        objectsCompared.Difference += ", ";
                //    objectsCompared.Difference = objectsCompared.Difference += "Properties";
                //}

                if (!navObject1.IsCodeEqual(navObject2))
                {
                    objectsCompared.CodeEqual = false;
                    if (!string.IsNullOrEmpty(objectsCompared.Difference))
                        objectsCompared.Difference += ", ";
                    objectsCompared.Difference = objectsCompared.Difference += "Code";
                }
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
    }
}
