using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompare.Editor
{
    public delegate void FileWatcherEventHandler(object source, FileWatcherEventArgs e);
    public delegate void FileWatcherErrorEventHandler(object source, FileWatcherErrorEventArgs e);

    public class FileWatcher
    {

        public event FileWatcherEventHandler OnFileChanged;
        public event FileWatcherErrorEventHandler OnFileError;

        private FileSystemWatcher _systemWatcher = new FileSystemWatcher();
        private string _filePathA = string.Empty;
        private string _filePathB = string.Empty;

        public FileWatcher(string watchPath)
        {
            if (string.IsNullOrEmpty(watchPath))
                throw new Exception("Watch Path cannot be empty or blank.");

            _systemWatcher.Path = watchPath;
            _systemWatcher.IncludeSubdirectories = false;

            _systemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName; // NotifyFilters.CreationTime

        }

        public void StartWatching(string filePathA, string filePathB)
        {
            _filePathA = filePathA;
            _filePathB = filePathB;

            // Add event handlers.
            _systemWatcher.Changed += new FileSystemEventHandler(OnChanged);
            _systemWatcher.Created += new FileSystemEventHandler(OnChanged);
            _systemWatcher.Renamed += new RenamedEventHandler(OnRenamed);
            _systemWatcher.Error += new ErrorEventHandler(OnError);

            // Begin Watching.
            _systemWatcher.EnableRaisingEvents = true;
        }

        public void StopWatching()
        {
            // Stop Watching.
            _systemWatcher.EnableRaisingEvents = false;
        }

        public void ResumeWatching()
        {
            // Resume Watching.
            _systemWatcher.EnableRaisingEvents = true;
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            RaiseErrorEvent(e.GetException());
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // File is changed first but normally name of file is different because its a tmp file
            if (e.FullPath == _filePathA)
                RaiseEventFileChanged(_filePathA);

            if (e.FullPath == _filePathB)
                RaiseEventFileChanged(_filePathB);
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // The tmp file is renamed and this event occours. This is what normally imports the file.
            if (e.FullPath == _filePathA)
                RaiseEventFileChanged(_filePathA);

            if (e.FullPath == _filePathB)
                RaiseEventFileChanged(_filePathB);
        }

        private void RaiseEventFileChanged(string filePath)
        {
            this.OnFileChanged?.Invoke(this, new FileWatcherEventArgs(filePath));
        }

        private void RaiseErrorEvent(Exception ex)
        {
            this.OnFileError?.Invoke(this, new FileWatcherErrorEventArgs(ex));
        }
    }

    public class FileWatcherEventArgs : EventArgs
    {
        public FileWatcherEventArgs(string fileChangedPath)
        {
            this.FileChangedPath = fileChangedPath;
        }
        public string FileChangedPath { get; private set; }
    }

    public class FileWatcherErrorEventArgs : EventArgs
    {
        public FileWatcherErrorEventArgs(Exception ex)
        {
            this.Exception = ex;
        }
        public Exception Exception { get; private set; }
    }
}
