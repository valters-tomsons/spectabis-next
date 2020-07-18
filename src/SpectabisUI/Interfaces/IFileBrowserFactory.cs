using System.Threading.Tasks;

namespace SpectabisUI.Interfaces
{
    public interface IFileBrowserFactory
    {
        Task<string> BeginGetDirectoryPath(string title, string path = null);
        Task<string> BeginGetSingleFilePath(string title, string initialDirectory = null);
    }
}