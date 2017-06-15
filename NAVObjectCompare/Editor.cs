using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using NAVObjectCompareWinClient.FileNotification;

namespace NAVObjectCompare
{
    public delegate void EditorEventHandler(object source, EditorEventArgs e);
    public class Editor
    {
        public event EditorEventHandler OnReCompareObject;

        FileWatcher _watcher = null;
        string _editorExePath = string.Empty;
        public Dictionary<string, NavObject> ObjectsA { get; set; }
        public Dictionary<string, NavObject> ObjectsB { get; set; }

        public Editor(string editorExePath)
        {
            _editorExePath = editorExePath;

            _watcher = new FileWatcher(GetObjectFileFolder());
            _watcher.OnFileChanged += _watcher_OnFileChanged;
        }

        public void OpenEditor(ObjectsCompared objectsCompared)
        {
            _watcher.StopWatching();

            string filePathA = CreateAndExportObject(objectsCompared, this.ObjectsA, "A");
            string filePathB = CreateAndExportObject(objectsCompared, this.ObjectsB, "B");

            Start(filePathA, filePathB);

            _watcher.StartWatching(filePathA, filePathB);
        }

        private string CreateAndExportObject(ObjectsCompared objectsCompared, Dictionary<string, NavObject> objects, string tag)
        { 
            string filePath = GetObjectFilePath(objectsCompared, tag);

            NavObject navObject = null;
            if (objects.TryGetValue(objectsCompared.InternalId, out navObject))
            {
                ExportFile(navObject.ObjectLines, filePath);
            }
            else
            {
                ExportFile(new List<string>(), filePath); // Empty file
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
            using (StreamWriter textObject = new StreamWriter(filePath, false, Encoding.Default))
            {
                foreach (string line in lines)
                {
                    textObject.WriteLine(line);
                }
            }
        }

        private string GetObjectFilePath(ObjectsCompared objectsCompared, string tag)
        {            
            string fileName = string.Format("{0}-{1}-{2}.txt", objectsCompared.Type, objectsCompared.Id, tag);
            return Path.Combine(GetObjectFileFolder(), fileName);
        }

        public static string GetObjectFileFolder()
        {
            string tempPath = System.IO.Path.GetTempPath();
            string dateFolder = string.Format("{0}{1}{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string objectFilePath = Path.Combine(tempPath, dateFolder);

            if (!Directory.Exists(objectFilePath))
            {
                Directory.CreateDirectory(objectFilePath);
            }
            return objectFilePath;
        }

        // Events
        private void _watcher_OnFileChanged(object source, FileWatcherEventArgs e)
        {
            ReImportObject(e.FileChangedPath);
        }

        private void ReImportObject(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string[] parts = fileName.Split('-');

            if (parts.Length != 3)
                return; // Not correct filename

            string internalId = string.Format("{0}-{1}", parts[0].ToUpper(), parts[1]);

            switch (parts[2])
            {
                case "A":
                    GetObjectAndImport(ObjectsA, internalId, filePath);
                    break;
                case "B":
                    GetObjectAndImport(ObjectsB, internalId, filePath);
                    break;
            }

            File.Delete(filePath);
        }

        private void GetObjectAndImport(Dictionary<string, NavObject> prevObjects, string internalId, string filePath)
        {
            // Add New Non Exsisting Object
            ObjectFile objectFile = new ObjectFile(filePath);
            Dictionary<string, NavObject> newObjects = objectFile.Run();

            AddOrUpdateObject(prevObjects, internalId, newObjects);
        }

        private void AddOrUpdateObject(Dictionary<string, NavObject> prevObjects, string internalId, Dictionary<string, NavObject> newObjects)
        {
            if (newObjects.ContainsKey(internalId))
            {
                NavObject newObject = newObjects[internalId];
                if (prevObjects.ContainsKey(internalId))
                {
                    prevObjects[internalId] = newObject;
                }
                else
                {
                    prevObjects.Add(internalId, newObject);
                }
                // Fire Event
                if (OnReCompareObject != null)
                    OnReCompareObject(this, new EditorEventArgs(newObject));
            }
        }
    }

    public class EditorEventArgs : EventArgs
    {
        public EditorEventArgs(NavObject newObject)
        {
            this.NavObject = newObject;
        }
        public NavObject NavObject { get; private set;}
    }
}
