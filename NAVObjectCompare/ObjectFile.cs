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
        private enum ObjectSection { Empty, NewObject, ObjectProperties, Properties, Code };

        private string _filePath = string.Empty;

        private Dictionary<string, NavObject> _navObjects = new Dictionary<string, NavObject>(); 

        public ObjectFile(string filePath)
        {
            _filePath = filePath;
        }

        public Dictionary<string, NavObject> Run()
        {
            ObjectSection currObjectSection = ObjectSection.Empty;
            NavObject currNavObject = null;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                ObjectSection objectSection = FindObjectKeyWord(line);
                if (objectSection != ObjectSection.Empty)
                    currObjectSection = objectSection;

                ProcessLine(line, currObjectSection, ref currNavObject);
            }

            return _navObjects;
        }

        private ObjectSection FindObjectKeyWord(string currentLine)
        {
            ObjectSection objectSection = ObjectSection.Empty;

            string[] parts = currentLine.Split(' ');

            foreach (string element in parts)
            {
                switch (element)
                {
                    case "OBJECT":
                        objectSection = ObjectSection.NewObject;
                        break;
                    case "OBJECT-PROPERTIES":
                        objectSection = ObjectSection.ObjectProperties;
                        break;
                    case "PROPERTIES":
                        objectSection = ObjectSection.Properties;
                        break;
                    case "CODE":
                        objectSection = ObjectSection.Code;
                        break;
                }
            }

            return objectSection;
        }

        private void ProcessLine(string line, ObjectSection objectSection, ref NavObject navObject)
        {
            switch (objectSection)
            {
                case ObjectSection.NewObject:
                    navObject = CheckCreateNewNavObject(line, objectSection, navObject);
                    break;
                case ObjectSection.ObjectProperties:
                    SetObjectProperties(line, objectSection, ref navObject);
                    break;
                case ObjectSection.Properties:
                    break;
                case ObjectSection.Code:
                    break;
            }

            navObject.AddLine(line);
        }

        private NavObject CheckCreateNewNavObject(string line, ObjectSection objectSection, NavObject navObject)
        {
            NavObject newNavObject = CreateNewObject(line, objectSection);
            if (newNavObject != null)
            {
                navObject = newNavObject;
                _navObjects.Add(newNavObject.InternalId, newNavObject);
            }

            return navObject;
        }

        private NavObject CreateNewObject(string line, ObjectSection objectSection)
        {
            string[] parts = line.Split(' ');

            if (objectSection != ObjectSection.NewObject)
                return null;

            if (parts.Length == 0)
                return null;

            if (parts[0] != "OBJECT")
                return null;

            NavObject navObject = new NavObject();
            navObject.Type = parts[1];
            navObject.Id = ObjectHelper.GetInt(parts[2]);
            navObject.Name = ObjectHelper.GetObjectName(line);

            return navObject;
        }

        private void SetObjectProperties(string line, ObjectSection objectSection, ref NavObject navObject)
        {
            if (objectSection != ObjectSection.ObjectProperties)
                return;

            string[] parts = line.Split('=');

            switch (ObjectHelper.RemoveIllChar(parts[0]))
            {
                case "Date":
                    navObject.StringDate = ObjectHelper.RemoveIllChar(parts[1]);
                    break;
                case "Time":
                    navObject.StringTime = ObjectHelper.RemoveIllChar(parts[1]);
                    break;
                case "Modified":
                    navObject.Modified = ObjectHelper.GetBool(parts[1]);
                    break;
                case "Version List":
                    navObject.VersionList = ObjectHelper.GetVersionList(line, parts[0]);
                    break;
            }
        }
    }
}
