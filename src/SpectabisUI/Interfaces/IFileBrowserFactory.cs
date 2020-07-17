using System.Threading.Tasks;

namespace SpectabisUI.Interfaces
{
    public interface IFileBrowserFactory
    {
        Task<string> BeginGetSingleFilePath(string title, string initialDirectory);
    }
}