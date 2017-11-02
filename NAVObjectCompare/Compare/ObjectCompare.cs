using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAVObjectCompare.Models;
using NAVObjectCompare.Helpers;

namespace NAVObjectCompare.Compare
{
    public delegate void CompareEventHandler(int percentCompleted);

    public class ObjectCompare
    {

        public event CompareEventHandler OnCompared;
        public Dictionary<string, NavObject> NavObjectsA { get { return _navObjectsA; } }
        public Dictionary<string, NavObject> NavObjectsB { get { return _navObjectsB; } }

        private enum ObjectPart { Empty, NewObject, ObjectProperties, Properties, Code };
        private Dictionary<string, NavObject> _navObjectsA = null;
        private Dictionary<string, NavObject> _navObjectsB = null;
        private Dictionary<string, NavObjectsCompared> _objectsComparedDict = new Dictionary<string, NavObjectsCompared>();

        private int _counter = 0;
        private int _totalObjectsToCompare = 0;

        public void RunCompare()
        {
            ObjectFile fileA = new ObjectFile(this.CompareFilePathA);
            fileA.OnFileNewLineRead += FileA_OnFileNewLineRead;
            _navObjectsA = fileA.Run();

            ObjectFile fileB = new ObjectFile(this.CompareFilePathB);
            fileB.OnFileNewLineRead += FileB_OnFileNewLineRead;
            _navObjectsB = fileB.Run();

            _counter = 0;
            _totalObjectsToCompare = _navObjectsA.Keys.Count + _navObjectsB.Keys.Count;

            FindDifferencesA();
            FindDifferencesB();
        }

        private void FileB_OnFileNewLineRead(int percentCompleted)
        {
            OnCompared?.Invoke(percentCompleted);
        }

        private void FileA_OnFileNewLineRead(int percentCompleted)
        {
            OnCompared?.Invoke(percentCompleted);
        }

        public List<NavObjectsCompared> GetList()
        {
            return _objectsComparedDict.Values.ToList<NavObjectsCompared>();
        }
        public string CompareFilePathA { get; set; }
        public string CompareFilePathB { get; set; }

        #region Public Methods

        public void FindDifferencesA()
        {
            foreach (string internalId in _navObjectsA.Keys)
            {
                FindDifferencesA(internalId);

                _counter++;
                FireCompareEvent();
            }
        }


        public void FindDifferencesA(string internalId)
        {
            NavObject navObjectA = ObjectHelper.GetDictValue(_navObjectsA, internalId);
            NavObject navObjectB = ObjectHelper.GetDictValue(_navObjectsB, internalId);

            NavObjectsCompared objectsCompared = new NavObjectsCompared(internalId)
            {
                Id = navObjectA.Id,
                Type = navObjectA.Type,
                Name = navObjectA.Name
            };

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

                _counter++;
                FireCompareEvent();
            }
        }

        public void FindDifferencesB(string internalId)
        {
            NavObject navObjectB = ObjectHelper.GetDictValue(_navObjectsB, internalId);
            NavObject navObjectA = ObjectHelper.GetDictValue(_navObjectsA, internalId);

            NavObjectsCompared objectsCompared = new NavObjectsCompared(internalId)
            {
                Id = navObjectB.Id,
                Type = navObjectB.Type,
                Name = navObjectB.Name
            };


            GetDifference(navObjectB, navObjectA, objectsCompared);

            SetAValues(navObjectA, objectsCompared);
            SetBValues(navObjectB, objectsCompared);

            if (!_objectsComparedDict.ContainsKey(internalId))
                _objectsComparedDict.Add(internalId, objectsCompared);
            else
                _objectsComparedDict[internalId] = objectsCompared;

        }

        public bool IsEditedA()
        {
            bool isEdited = false;

            foreach (string internalId in _navObjectsA.Keys)
            {
                NavObject navObjectA = ObjectHelper.GetDictValue(_navObjectsA, internalId);
                if(navObjectA.IsEdited)
                {
                    isEdited = true;
                    break;
                }
            }

            return isEdited;
        }

        public bool IsEditedB()
        {
            bool isEdited = false;

            foreach (string internalId in _navObjectsB.Keys)
            {
                NavObject navObjectB = ObjectHelper.GetDictValue(_navObjectsB, internalId);
                if (navObjectB.IsEdited)
                {
                    isEdited = true;
                    break;
                }
            }

            return isEdited;
        }

        #endregion Public Methods

        #region Private Methods

        private void GetDifference(NavObject navObject1, NavObject navObject2, NavObjectsCompared objectsCompared)
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


        private void FireCompareEvent()
        {
            if (this.OnCompared != null)
            {
                double percentageCompleted = 0;
                if (_totalObjectsToCompare != 0)
                    percentageCompleted = (((double)_counter / (double)_totalObjectsToCompare) * 100);
                else
                    percentageCompleted = 100;

                this.OnCompared((int)percentageCompleted);
            }
        }

        private static void SetAValues(NavObject navObjectA, NavObjectsCompared objectsCompared)
        {
            if (navObjectA != null)
            {
                objectsCompared.StringDateA = navObjectA.StringDate;
                objectsCompared.StringTimeA = navObjectA.StringTime;
                objectsCompared.VersionListA = navObjectA.VersionList;
                objectsCompared.NoOfLinesA = navObjectA.ObjectLines.Count;
            }
        }

        private static void SetBValues(NavObject navObjectB, NavObjectsCompared objectsCompared)
        {
            if (navObjectB != null)
            {
                objectsCompared.StringDateB = navObjectB.StringDate;
                objectsCompared.StringTimeB = navObjectB.StringTime;
                objectsCompared.VersionListB = navObjectB.VersionList;
                objectsCompared.NoOfLinesB = navObjectB.ObjectLines.Count;
            }
        }

        #endregion Private Methods
    }
}
