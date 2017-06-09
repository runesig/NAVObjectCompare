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
        private string _filePath = string.Empty;

        private Dictionary<string, NavObject> _navObjects = new Dictionary<string, NavObject>(); 

        public ObjectFile(string filePath)
        {
            _filePath = filePath;
        }

        public Dictionary<string, NavObject> Run()
        {
            if (string.IsNullOrEmpty(_filePath))
                return new Dictionary<string, NavObject>();

            ObjectSection currObjectSection = ObjectSection.Unknown;
            NavObject currNavObject = null;

            var lines = File.ReadAllLines(_filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                ObjectSection objectSection = ObjectHelper.FindObjectSection(lines[i]);
                if (objectSection != ObjectSection.Unknown)
                    currObjectSection = objectSection;

                ProcessLine(lines[i], currObjectSection, ref currNavObject);
            }

            return _navObjects;
        }


        private void ProcessLine(string line, ObjectSection objectSection, ref NavObject navObject)
        {
            switch (objectSection)
            {
                case ObjectSection.Object:
                    navObject = CreateNewObject(line, objectSection, navObject);
                    break;
                case ObjectSection.ObjectProperties:
                    SetObjectProperties(line, objectSection, ref navObject);
                    navObject.ObjectProperties.Add(line);
                    break;
                case ObjectSection.Properties:
                    navObject.Properties.Add(line);
                    break;
                case ObjectSection.Fields:
                    navObject.Fields.Add(line);
                    break;
                case ObjectSection.Keys:
                    navObject.Keys.Add(line);
                    break;
                case ObjectSection.FieldGroups:
                    navObject.FieldGroups.Add(line);
                    break;
                case ObjectSection.Code:
                    navObject.Code.Add(line);
                    break;
            }

            navObject.ObjectLines.Add(line);
        }

        private NavObject CreateNewObject(string line, ObjectSection objectSection, NavObject navObject)
        {
            NavObject newNavObject = NewObject(line, objectSection);
            if (newNavObject != null)
            {
                navObject = newNavObject;
                _navObjects.Add(newNavObject.InternalId, newNavObject);
            }

            return navObject;
        }

        private NavObject NewObject(string line, ObjectSection objectSection)
        {
            string[] parts = line.Split(' ');

            if (objectSection != ObjectSection.Object)
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
