using System.IO;
using System.Threading;
using Crosstales.FB;
using Sirenix.Utilities;
using UnityEngine;

namespace Source
{
    public class FileSystemService
    {
        private readonly string _saveDirectory;
        
        public FileSystemService()
        {
            _saveDirectory = BE2_Paths.TranslateMarkupPath(BE2_Paths.SavedCodesPath);
            if (!Directory.Exists(_saveDirectory))
                Directory.CreateDirectory(_saveDirectory);
        }

        public string OpenSaveFile()
        {
            return FileBrowser.OpenSingleFile("Выберите файл", _saveDirectory, "BE2");
        }
    }
}