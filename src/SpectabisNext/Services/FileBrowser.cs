using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using SpectabisLib.Helpers;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class FileBrowser : IFileBrowserFactory
    {
        public async Task<string> BeginGetSingleFilePath(string title, string initialDirectory = null)
        {
            var dialog = new OpenFileDialog
            {
                Title = title,
                AllowMultiple = false,
                Directory = SystemDirectories.HomeFolder
            };

            if(!string.IsNullOrEmpty(initialDirectory))
            {
                dialog.Directory = initialDirectory;
            }

            if (!Directory.Exists(initialDirectory))
            {
                dialog.Directory = SystemDirectories.HomeFolder;
            }

            var fileResult = await dialog.ShowAsync(new Window()).ConfigureAwait(false);

            if (fileResult == null || string.IsNullOrWhiteSpace(fileResult[0]))
            {
                return null;
            }

            return fileResult[0];
        }
    }
}