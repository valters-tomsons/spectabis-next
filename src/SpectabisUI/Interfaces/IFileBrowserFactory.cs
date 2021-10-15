using System.Threading.Tasks;
using Avalonia.Controls;

namespace SpectabisUI.Interfaces
{
    public interface IFileBrowserFactory
    {
        Task<string> BeginGetDirectoryPath(string title, string path = null);
        Task<string> BeginGetSingleFilePath(string title, string initialDirectory = null);
        void Internals_SetRootWindow(Window window);
    }
}