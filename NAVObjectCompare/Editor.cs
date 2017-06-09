using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace NAVObjectCompare
{
    public class Editor
    {
        string _editorExePath = string.Empty;
        public Dictionary<string, NavObject> ObjectsA { get; set; }
        public Dictionary<string, NavObject> ObjectsB { get; set; }

        public Editor(string editorExePath)
        {
            _editorExePath = editorExePath;
        }
        public void OpenEditor(ObjectsCompared objectsCompared)
        {
            string filePathA = CreateAndExportObject(objectsCompared, this.ObjectsA, "A");
            string filePathB = CreateAndExportObject(objectsCompared, this.ObjectsB, "B");

            Start(filePathA, filePathB);
        }

        private string CreateAndExportObject(ObjectsCompared objectsCompared, Dictionary<string, NavObject> objects, string tag)
        { 
            string filePath = string.Empty;

            NavObject navObject = null;
            if (objects.TryGetValue(objectsCompared.InternalId, out navObject))
            {
                filePath = GetObjectFilePath(navObject, tag);
                ExportFile(navObject.ObjectLines, filePath);
            }

            return filePath;
        }

        private void Start(string filePathA, string filePathB)
        {
            string command = string.Empty;

            if(!string.IsNullOrEmpty(filePathA) && string.IsNullOrEmpty(filePathB))
                command = string.Format("{0}", filePathA);
            else if (string.IsNullOrEmpty(filePathA) && !string.IsNullOrEmpty(filePathB))
                command = string.Format("{0}", filePathB);
            else
                command = string.Format("{0} {1}", filePathA, filePathB);

            Process.Start(_editorExePath, command);
        }

        private void ExportFile(List<string> lines, string filePath)
        {
            using (StreamWriter textObject = new StreamWriter(filePath))
            {
                foreach (string line in lines)
                {
                    textObject.WriteLine(line);
                }
            }
        }

        private string GetObjectFilePath(NavObject navObject, string tag)
        {            
            string fileName = string.Format("{0}{1}.txt", navObject.InternalId, tag);
            return Path.Combine(GetObjectFileFolder(), fileName);
        }

        private string GetObjectFileFolder()
        {
            return System.IO.Path.GetTempPath();
        }
    }
}
