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
            NavObject.Difference difference = NavObject.Difference.None;

            if (navObject1.IsEqualTo(navObject2, out difference))
            {
                objectsCompared.Equal = true;
                objectsCompared.Difference = DifferenceDescription(NavObject.Difference.None);
            }
            else
            {
                objectsCompared.Equal = false;
                objectsCompared.Difference = DifferenceDescription(difference);
            }
        }

        private static void SetAValues(NavObject navObjectA, ObjectsCompared objectsCompared)
        {
            if (navObjectA != null)
            {
                objectsCompared.StringDateA = navObjectA.StringDate;
                objectsCompared.StringTimeA = navObjectA.StringTime;
                objectsCompared.VersionListA = navObjectA.VersionList;
                objectsCompared.NoOfLinesA = navObjectA.LineCount();
            }
        }

        private static void SetBValues(NavObject navObjectB, ObjectsCompared objectsCompared)
        {
            if (navObjectB != null)
            {
                objectsCompared.StringDateB = navObjectB.StringDate;
                objectsCompared.StringTimeB = navObjectB.StringTime;
                objectsCompared.VersionListB = navObjectB.VersionList;
                objectsCompared.NoOfLinesB = navObjectB.LineCount();
            }
        }

        private NavObject GetDictValue(Dictionary<string, NavObject> dict, string key)
        {
            NavObject navObject = null;
            if (!dict.TryGetValue(key, out navObject))
                return null;

            return navObject;
        }

        private string DifferenceDescription(NavObject.Difference difference)
        {
            string differenceDescription = string.Empty;

            switch (difference)
            {
                case NavObject.Difference.Unexisting:
                    differenceDescription = "Object does not exists in file.";
                    break;
                case NavObject.Difference.Id:
                    differenceDescription = "Object Ids are not equal.";
                    break;
                case NavObject.Difference.Name:
                    differenceDescription = "Object names are not equal.";
                    break;
                case NavObject.Difference.DateOrTime:
                    differenceDescription = "Date or time is not equal.";
                    break;
                case NavObject.Difference.ModifiedFlag:
                    differenceDescription = "Modified flag is not equal.";
                    break;
                case NavObject.Difference.Code:
                    differenceDescription = "Code or comments are different.";
                    break;
                default:
                    differenceDescription = "Equal.";
                    break;
            }

            return differenceDescription;
        }
    }
}
