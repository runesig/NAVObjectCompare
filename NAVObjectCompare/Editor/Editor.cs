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
    public delegate void EditorErrorEventHandler(object source, EditorErrorEventArgs e);

    public class Editor
    {
        public event EditorEventHandler OnReCompareObject;
        public event EditorErrorEventHandler OnEditorError;

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
            _watcher.OnFileError += _watcher_OnFileError;
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
            int noOfRetries = 0;
            string internalIdProcessed = string.Empty;

            ReImportObject(e.FileChangedPath, ref noOfRetries, ref internalIdProcessed);
        }

        private void _watcher_OnFileError(object source, FileWatcherErrorEventArgs e)
        {
            OnEditorError?.Invoke(this, new EditorErrorEventArgs(e.Exception)); // Redirect Exception
        }

        private void ReImportObject(string filePath, ref int noOfRetries, ref string internalIdProcessed)
        {
            if (!string.IsNullOrEmpty(internalIdProcessed))
                return; // Object is alread processed

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (!IsFileExists(fileInfo))
                    return;

                if (IsFileLocked(fileInfo))
                {
                    if (noOfRetries == 10)
                        throw new Exception("Timed out reimporting object.");

                    System.Threading.Thread.Sleep(1000);
                    noOfRetries++;
                    ReImportObject(filePath, ref noOfRetries, ref internalIdProcessed);
                    return;
                }

                SplitFileName(filePath, out string[] parts);

                internalIdProcessed = string.Format("{0}-{1}", parts[0].ToUpper(), parts[1]);

                switch (parts[2])
                {
                    case _tagA:
                        GetObjectAndImport(ObjectsA, internalIdProcessed, filePath);
                        break;
                    case _tagB:
                        GetObjectAndImport(ObjectsB, internalIdProcessed, filePath);
                        break;
                }

                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                OnEditorError?.Invoke(this, new EditorErrorEventArgs(ex));
            }
        }

        private void SplitFileName(string filePath, out string[] parts)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            parts = fileName.Split('-');

            if (parts.Length != 3)
                throw new Exception(string.Format("Incorrect file name: {0}. Reimport not possible", filePath));
        }

        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        private bool IsFileExists(FileInfo file)
        {
            return file.Exists;
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

    public class EditorErrorEventArgs : EventArgs
    {
        public EditorErrorEventArgs(Exception ex)
        {
            this.Exception = ex;
        }
        public Exception Exception { get; private set; }
    }
}
