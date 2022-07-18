using Petroineos.Interfaces;
using System.IO;

namespace Petroineos.Services
{
    public class IOService : IIOService
    {
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public string[] DirectoryGetFiles(string path, string searchOption)
        {
            return Directory.GetFiles(path, searchOption);
        }

        public bool FileExists (string fileNameWithPath)
        {
            return File.Exists(fileNameWithPath);
        }

        public void DeleteFile(string fileNameWithPath)
        {
            File.Delete(fileNameWithPath);
        }
    }
}
