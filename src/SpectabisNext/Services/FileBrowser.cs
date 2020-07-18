using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using SpectabisLib.Helpers;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class FileBrowser : IFileBrowserFactory
    {
        public async Task<string> BeginGetSingleFilePath(string title, string path = null)
        {
            var dialog = new OpenFileDialog
            {
                Title = title,
                AllowMultiple = false,
                Directory = SystemDirectories.HomeFolder
            };

            if(!string.IsNullOrEmpty(path))
            {
                dialog.Directory = path;
            }

            if(File.Exists(path))
            {
                var fileName = Path.GetFileNameWithoutExtension(path);
                dialog.Directory = path.Replace(fileName, string.Empty);
            }
            else if(Directory.Exists(path))
            {
                dialog.Directory = path;
            }

            var fileResult = await dialog.ShowAsync(new Window()).ConfigureAwait(true);

            if (fileResult == null || string.IsNullOrWhiteSpace(fileResult[0]))
            {
                return null;
            }

            return fileResult[0];
        }
    }
}