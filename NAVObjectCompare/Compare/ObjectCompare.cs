using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAVObjectCompare.Models;
using NAVObjectCompare.Helpers;
using System.Collections.ObjectModel;

namespace NAVObjectCompare.Compare
{
    public delegate void CompareEventHandler(int percentCompleted);

    public class ObjectCompare
    {
        // Enum
        private enum ObjectPart { Empty, NewObject, ObjectProperties, Properties, Code };

        // Events
        public event CompareEventHandler OnCompared;


        public Dictionary<string, NavObject> NavObjectsA { get { return _navObjectsA; } }
        public Dictionary<string, NavObject> NavObjectsB { get { return _navObjectsB; } }
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

        #region Serialize

        public void Serialize(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);

            SerializeNavObjectsA(ref writer);

            SerializeNavObjectsB(ref writer);

            SerializeNavObjectsCompared(ref writer);

            writer.Flush();
        }

        private void SerializeNavObjectsCompared(ref BinaryWriter writer)
        {
            // NavObjectsCompared
            writer.Write(_objectsComparedDict.Count);
            foreach (var obj in _objectsComparedDict)
            {
                writer.Write(obj.Key); // Key
                NavObjectsCompared compared = (NavObjectsCompared)obj.Value; // Value
                compared.Serialize(ref writer);
            }
        }

        private void SerializeNavObjectsB(ref BinaryWriter writer)
        {
            // B
            writer.Write(_navObjectsB.Count);
            foreach (var obj in _navObjectsB)
            {
                writer.Write(obj.Key); // Key
                NavObject navObject = (NavObject)obj.Value; // Value
                navObject.Serialize(ref writer);
            }
        }

        private void SerializeNavObjectsA(ref BinaryWriter writer)
        {
            // A
            writer.Write(_navObjectsA.Count);
            foreach (var obj in _navObjectsA)
            {
                writer.Write(obj.Key); // Key
                NavObject navObject = (NavObject)obj.Value; // Value
                navObject.Serialize(ref writer);
            }
        }

        public void Deserialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            DeserializeNavObjectsA(ref reader);

            DeserializeNavObjectsB(ref reader);

            DeserializeNavObjectsCompared(ref reader);

        }

        private void DeserializeNavObjectsCompared(ref BinaryReader reader)
        {
            int countCompared = reader.ReadInt32();
            _objectsComparedDict = new Dictionary<string, NavObjectsCompared>(countCompared);

            for (int n = 0; n < countCompared; n++)
            {
                var key = reader.ReadString(); // Key
                NavObjectsCompared compared = NavObjectsCompared.Desserialize(ref reader);
                _objectsComparedDict.Add(key, compared);
            }
        }

        private void DeserializeNavObjectsB(ref BinaryReader reader)
        {
            // B
            int countB = reader.ReadInt32();
            _navObjectsB = new Dictionary<string, NavObject>(countB);

            for (int n = 0; n < countB; n++)
            {
                var key = reader.ReadString(); // Key
                NavObject navObj = NavObject.Desserialize(ref reader);
                _navObjectsB.Add(key, navObj);
            }
        }

        private void DeserializeNavObjectsA(ref BinaryReader reader)
        {
            // A
            int countA = reader.ReadInt32();
            _navObjectsA = new Dictionary<string, NavObject>(countA);

            for (int n = 0; n < countA; n++)
            {
                var key = reader.ReadString(); // Key
                NavObject navObj = NavObject.Desserialize(ref reader);
                _navObjectsA.Add(key, navObj);
            }
        }

        #endregion Serialize

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

        public ObservableCollection<NavObjectsCompared> GetObservableCollection()
        {
            return new ObservableCollection<NavObjectsCompared>(_objectsComparedDict.Values);
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

            NavObjectsCompared newObjectsCompared = new NavObjectsCompared(internalId)
            {
                Id = navObjectA.Id,
                Type = navObjectA.Type,
                Name = navObjectA.Name,
                Edited = (navObjectA == null ? false : navObjectA.IsEdited) || (navObjectB == null ? false : navObjectB.IsEdited)
            };

            GetDifference(navObjectA, navObjectB, newObjectsCompared);

            SetAValues(navObjectA, newObjectsCompared);
            SetBValues(navObjectB, newObjectsCompared);

            AddObjectComparedToDictionary(internalId, newObjectsCompared);
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

            NavObjectsCompared newObjectsCompared = new NavObjectsCompared(internalId)
            {
                Id = navObjectB.Id,
                Type = navObjectB.Type,
                Name = navObjectB.Name,
                Edited = (navObjectB == null ? false : navObjectB.IsEdited) || (navObjectA == null ? false : navObjectA.IsEdited)
            };


            GetDifference(navObjectB, navObjectA, newObjectsCompared);

            SetAValues(navObjectA, newObjectsCompared);
            SetBValues(navObjectB, newObjectsCompared);

            AddObjectComparedToDictionary(internalId, newObjectsCompared);
        }

        private void AddObjectComparedToDictionary(string internalId, NavObjectsCompared newObjectsCompared)
        {
            if (!_objectsComparedDict.ContainsKey(internalId))
                _objectsComparedDict.Add(internalId, newObjectsCompared);
            else
            {
                if (_objectsComparedDict.TryGetValue(internalId, out NavObjectsCompared prevObjectsCompared))
                {
                    newObjectsCompared.Selected = prevObjectsCompared.Selected;
                }
                _objectsComparedDict[internalId] = newObjectsCompared;
            }
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

            if (!ObjectExists(navObject1, navObject2, ref objectsCompared))
                return;

            if (!ObjectIsEqual(navObject1, navObject2, ref objectsCompared))
                return;

            objectsCompared.CodeEqual = true;
            objectsCompared.ObjectPropertiesEqual = true;
            objectsCompared.Status = NavObjectsCompared.EqualStatus.Equal;
        }

        private bool ObjectExists(NavObject navObject1, NavObject navObject2, ref NavObjectsCompared objectsCompared)
        {
            if (!navObject1.IsExisting(navObject2))
            {
                objectsCompared.CodeEqual = false;
                objectsCompared.ObjectPropertiesEqual = false;
                objectsCompared.Status = NavObjectsCompared.EqualStatus.Unexisting;
                objectsCompared.Comment = "Object does not exists";

                return false;
            }

            return true;
        }

        private bool ObjectIsEqual(NavObject navObject1, NavObject navObject2, ref NavObjectsCompared objectsCompared)
        {
            string comment = string.Empty;
            objectsCompared.ObjectPropertiesEqual = true;
            objectsCompared.CodeEqual = true;

            if (!navObject1.IsEqualTo(navObject2))
            {
                objectsCompared.Status = NavObjectsCompared.EqualStatus.Unequal;

                // Do More Analysis
                if (!navObject1.IsObjectPropertiesEqual(navObject2))
                {
                    objectsCompared.ObjectPropertiesEqual = false;
                    AddToComment(ref comment, "Date, Time or Version");
                }

                if (!navObject1.IsCodeEqual(navObject2))
                {
                    objectsCompared.CodeEqual = false;
                    AddToComment(ref comment, "Code");
                }
                objectsCompared.Comment = comment;

                return false;
            }

            return true;
        }

        private void AddToComment(ref string comment, string value)
        {
            if (!string.IsNullOrEmpty(comment))
                comment = string.Format("{1}, {0}", comment, value);
            else
                comment = value;
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
