using System.Linq;
using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisUI.ViewModels;
using SpectabisUI.Interfaces;
using Avalonia.Interactivity;
using SpectabisLib.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SpectabisUI.Pages
{
    public class Settings : UserControl, IPage
    {
        public string PageTitle => "Settings";
        public bool ShowInTitlebar => true;
        public bool HideTitlebar => false;
        public bool ReloadOnNavigation => false;

        private Button AddScanButton { get; set; }
        private Button RemoveScanButton { get; set; }
        private ListBox DirectoryList { get; set; }

        private readonly SettingsViewModel _viewModel;
        private readonly IFileBrowserFactory _fileBrowser;
        private readonly ISettingsController _controller;

        [Obsolete("XAMLIL placeholder", true)]
        public Settings()
        {
        }

        public Settings(SettingsViewModel viewModel, IFileBrowserFactory fileBrowser, ISettingsController controller)
        {
            InitializeComponent();
            RegisterChildren();

            _viewModel = viewModel;
            DataContext = _viewModel;

            _fileBrowser = fileBrowser;
            _controller = controller;

            _viewModel.PropertyChanged += OnViewModelUpdated;
        }

        private async void OnViewModelUpdated(object sender, PropertyChangedEventArgs e)
        {
            await UpdateOptions().ConfigureAwait(true);
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void RegisterChildren()
        {
            AddScanButton = this.FindControl<Button>(nameof(AddScanButton));
            RemoveScanButton = this.FindControl<Button>(nameof(RemoveScanButton));
            DirectoryList = this.FindControl<ListBox>(nameof(DirectoryList));

            AddScanButton.Click += AddScanClick;
            RemoveScanButton.Click += RemoveScanClick;
        }

        private async void AddScanClick(object sender, RoutedEventArgs e)
        {
            var target = await _fileBrowser.BeginGetDirectoryPath("Select directory to scan").ConfigureAwait(true);
            var newDirs = await _controller.AppendScanDirectory(target).ConfigureAwait(true);

            if(newDirs != null)
            {
                _viewModel.ScanDirectories = newDirs.ToList();
            }
        }

        private async void RemoveScanClick(object sender, RoutedEventArgs e)
        {
            var selected = DirectoryList.SelectedItem as string;
            var newDirs = await _controller.RemoveScanDirectory(selected).ConfigureAwait(true);
            _viewModel.ScanDirectories = newDirs.ToList();
        }

        private async Task UpdateOptions()
        {
            await _controller.UpdateOptions(_viewModel.Telemetry, _viewModel.Discord).ConfigureAwait(true);
        }
    }
}