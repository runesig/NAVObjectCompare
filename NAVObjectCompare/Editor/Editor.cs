using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using NAVObjectCompare.Compare;
using NAVObjectCompare.Editor;
using NAVObjectCompare.Models;
using NAVObjectCompare.ExportObjects;

namespace NAVObjectCompare.Editor
{
    public delegate void EditorEventHandler(object source, EditorEventArgs e);

    public class Editor
    {
        public event EditorEventHandler OnReCompareObject;

        FileWatcher _watcher = null;
        string _editorExePath = string.Empty;
        public Dictionary<string, NavObject> ObjectsA { get; set; }
        public Dictionary<string, NavObject> ObjectsB { get; set; }
        public const string _tagA = "A";
        public const string _tagB = "B";

        public Editor(string editorExePath)
        {
            _editorExePath = editorExePath;

            _watcher = new FileWatcher(ObjectExport.GetObjectFileFolder());
            _watcher.OnFileChanged += _watcher_OnFileChanged;
        }

        public void OpenEditor(NavObjectsCompared objectsCompared)
        {
            _watcher.StopWatching();

            string filePathA = ObjectExport.CreateAndExportObject(objectsCompared, this.ObjectsA, _tagA);
            string filePathB = ObjectExport.CreateAndExportObject(objectsCompared, this.ObjectsB, _tagB);

            Start(filePathA, filePathB);

            _watcher.StartWatching(filePathA, filePathB);
        }

        private void Start(string filePathA, string filePathB)
        {
            string command = string.Empty;

            if(!string.IsNullOrEmpty(filePathA) && string.IsNullOrEmpty(filePathB))
                command = string.Format("\"{0}\"", filePathA);
            else if (string.IsNullOrEmpty(filePathA) && !string.IsNullOrEmpty(filePathB))
                command = string.Format("\"{0}\"", filePathB);
            else
                command = string.Format("\"{0}\" \"{1}\"", filePathA, filePathB);

            if (!File.Exists(_editorExePath))
                throw new Exception(string.Format("Beyond Compare is not installed at {0}", _editorExePath));

            Process.Start(_editorExePath, command);
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
                case _tagA:
                    GetObjectAndImport(ObjectsA, internalId, filePath);
                    break;
                case _tagB:
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
                newObject.IsEdited = true;

                if (prevObjects.ContainsKey(internalId))
                {
                    prevObjects[internalId] = newObject;
                }
                else
                {
                    prevObjects.Add(internalId, newObject);
                }
                // Fire Event
                OnReCompareObject?.Invoke(this, new EditorEventArgs(newObject));
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
