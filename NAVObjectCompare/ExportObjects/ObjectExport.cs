using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAVObjectCompare.Models;

namespace NAVObjectCompare.ExportObjects
{
    public class ObjectExport
    {
        public static string CreateAndExportObject(NavObjectsCompared objectsCompared, Dictionary<string, NavObject> objects, string tag)
        {
            string filePath = GetObjectFilePath(objectsCompared, tag);

            if (objects.TryGetValue(objectsCompared.InternalId, out NavObject navObject))
            {
                ExportFile(navObject.ObjectLines, filePath);
            }
            else
            {
                ExportFile(new List<string>(), filePath); // Empty file
            }

            return filePath;
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

        private static void ExportFile(List<string> lines, string filePath)
        {
            using (StreamWriter textObject = new StreamWriter(filePath, false, Encoding.Default))
            {
                foreach (string line in lines)
                {
                    textObject.WriteLine(line);
                }
            }
        }

        private static string GetObjectFilePath(NavObjectsCompared objectsCompared, string tag)
        {
            string fileName = string.Format("{0}-{1}-{2}.txt", objectsCompared.Type, objectsCompared.Id, tag);
            return Path.Combine(GetObjectFileFolder(), fileName);
        }
    }
}
