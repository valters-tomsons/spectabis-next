using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using SpectabisLib.Helpers;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class FileBrowser : IFileBrowserFactory
    {
        public async Task<string> BeginGetDirectoryPath(string title, string path = null)
        {
            var dialog = new OpenFolderDialog()
            {
                Title = title,
                Directory = SystemDirectories.HomeFolder
            };

            var suggestedDirectory = ResolveDirectoryFromPath(path);
            if(suggestedDirectory != null)
            {
                dialog.Directory = suggestedDirectory;
            }

            var fileResult = await dialog.ShowAsync(new Window()).ConfigureAwait(true);
            if (string.IsNullOrWhiteSpace(fileResult))
            {
                return null;
            }

            return fileResult;
        }

        public async Task<string> BeginGetSingleFilePath(string title, string path = null)
        {
            var dialog = new OpenFileDialog
            {
                Title = title,
                AllowMultiple = false,
                Directory = SystemDirectories.HomeFolder
            };

            var suggestedDirectory = ResolveDirectoryFromPath(path);
            if(suggestedDirectory != null)
            {
                dialog.Directory = suggestedDirectory;
            }

            var fileResult = await dialog.ShowAsync(new Window()).ConfigureAwait(true);
            if (fileResult == null || string.IsNullOrWhiteSpace(fileResult[0]))
            {
                return null;
            }

            return fileResult[0];
        }

        private string ResolveDirectoryFromPath(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                return null;
            }

            if(File.Exists(path))
            {
                var fileName = Path.GetFileName(path);
                return path.Replace(fileName, string.Empty);
            }
            else if(Directory.Exists(path))
            {
                return path;
            }

            return null;
        }
    }
}