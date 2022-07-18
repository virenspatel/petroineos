namespace Petroineos.Interfaces
{
    public interface IIOService
    {
        bool DirectoryExists(string path);
        string[] DirectoryGetFiles(string path, string searchOption);
        bool FileExists(string fileNameWithPath);
        void DeleteFile(string fileNameWithPath);
    }
}
