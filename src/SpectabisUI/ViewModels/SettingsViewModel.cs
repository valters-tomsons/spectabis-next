using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;
using SpectabisLib.Interfaces;

namespace SpectabisUI.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        private readonly IConfigurationLoader _configuration;
        private IList<string> _directories;

        public SettingsViewModel(IConfigurationLoader configuration)
        {
            _configuration = configuration;
            _directories = _configuration.Directories.GameScanDirectories.ToList();
        }

        public IList<string> ScanDirectories { get => _directories; set => this.RaiseAndSetIfChanged(ref _directories, value); }
    }
}