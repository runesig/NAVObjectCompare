using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NAVObjectCompare
{
    public class ObjectFile
    {
        private enum ObjectPart { Empty, NewObject, ObjectProperties, Properties, Code };

        private string _filePath = string.Empty;

        private List<NAVObject> _navObjects = new List<NAVObject>();
        private NAVObject _currentNAVObject = null;
        private string _currentNAVObjectDate = string.Empty;
        private string _currentNAVObjectTime = string.Empty;

        private ObjectPart _currentObjectPart = ObjectPart.Empty;

        public ObjectFile(string filePath)
        {
            _filePath = filePath;
        }

        public List<NAVObject> Run()
        {
            string currentLine = string.Empty;
            string prevLine = string.Empty;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                prevLine = currentLine;
                currentLine = line;

                SearchObjectKeyWord(currentLine, prevLine);
            }

            return _navObjects;
        }

        private void SearchObjectKeyWord(string currentLine, string prevLine)
        {
            string[] parts = currentLine.Split(' ');

            foreach (string element in parts)
            {
                switch (element)
                {
                    case "OBJECT":
                        AddNewNavObject();
                        break;
                    case "OBJECT-PROPERTIES":
                        AddObjectProperties();
                        break;
                    case "PROPERTIES":
                        AddProperties();
                        break;
                    case "CODE":
                        AddCode();
                        break;
                }
            }

            ProcessLine(currentLine);
        }

        private void ProcessLine(string line)
        {
            switch (_currentObjectPart)
            {
                case ObjectPart.NewObject:
                    SetObjectIdAndName(line);
                    break;
                case ObjectPart.ObjectProperties:
                    SetObjectProperties(line);
                    break;
                case ObjectPart.Properties:
                    break;
                case ObjectPart.Code:
                    break;
            }

            _currentNAVObject.AddLine(line);
        }

        private void AddNewNavObject()
        {
            // New Object
            _currentObjectPart = ObjectPart.NewObject;
            _currentNAVObject = null;
            _currentNAVObjectDate = string.Empty;
            _currentNAVObjectTime = string.Empty;

            NAVObject navObject = new NAVObject();
            _navObjects.Add(navObject);

            // Set Current Object
            _currentNAVObject = navObject;
        }

        private void AddObjectProperties()
        {
            // Object Properties
            _currentObjectPart = ObjectPart.ObjectProperties;
        }

        private void AddProperties()
        {
            // Properties
            _currentObjectPart = ObjectPart.Properties;
        }
        private void AddCode()
        {
            // Code
            _currentObjectPart = ObjectPart.Code;
        }

        private void SetObjectIdAndName(string line)
        {
            string[] parts = line.Split(' ');

            if (_currentObjectPart != ObjectPart.NewObject)
                return;

            if (parts.Length == 0)
                return;

            if (parts[0] != "OBJECT")
                return;

            _currentNAVObject.Type = parts[1];
            _currentNAVObject.Id = ObjectHelper.GetInt(parts[2]);
            _currentNAVObject.Name = ObjectHelper.GetObjectName(line);
        }

        private void SetObjectProperties(string line)
        {
            if (_currentObjectPart != ObjectPart.ObjectProperties)
                return;

            string[] parts = line.Split('=');

            switch (ObjectHelper.RemoveIllChar(parts[0]))
            {
                case "Date":
                    _currentNAVObjectDate = ObjectHelper.RemoveIllChar(parts[1]);
                    break;
                case "Time":
                    _currentNAVObjectTime = ObjectHelper.RemoveIllChar(parts[1]);
                    break;
                case "Modified":
                    _currentNAVObject.Modified = ObjectHelper.GetBool(parts[1]);
                    break;
                case "Version List":
                    _currentNAVObject.VersionList = ObjectHelper.GetVersionList(line, parts[0]);
                    break;
            }

            if ((!string.IsNullOrEmpty(_currentNAVObjectDate)) && (!string.IsNullOrEmpty(_currentNAVObjectTime)))
            {
                string stringDateTime = string.Format("{0} {1}", _currentNAVObjectDate, _currentNAVObjectTime);
                _currentNAVObject.DateTime = ObjectHelper.GetDateTime(stringDateTime);
            }

        }
    }
}
