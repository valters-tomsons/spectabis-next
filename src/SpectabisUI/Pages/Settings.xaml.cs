using System.Runtime.InteropServices;
using System.Reflection;
using System.Linq;
using System.IO;
using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisUI.ViewModels;
using SpectabisUI.Interfaces;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System.Collections.Generic;
using SpectabisLib.Interfaces;

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
        private readonly IConfigurationLoader _configuration;

        [Obsolete("XAMLIL placeholder", true)]
        public Settings()
        {
        }

        public Settings(SettingsViewModel viewModel, IFileBrowserFactory fileBrowser, IConfigurationLoader configuration)
        {
            InitializeComponent();
            RegisterChildren();

            _viewModel = viewModel;
            DataContext = _viewModel;

            _configuration = configuration;
            _fileBrowser = fileBrowser;
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

            if(string.IsNullOrEmpty(target))
            {
                return;
            }

            if(!Directory.Exists(target))
            {
                return;
            }

            var newDirs = _viewModel.ScanDirectories.Append(target);

            _configuration.Directories.GameScanDirectories = newDirs;
            await _configuration.WriteConfiguration(_configuration.Directories).ConfigureAwait(true);

            _viewModel.ScanDirectories = _configuration.Directories.GameScanDirectories.ToList();
        }

        private async void RemoveScanClick(object sender, RoutedEventArgs e)
        {
            var selected = DirectoryList.SelectedItem as string;
            var updated = _viewModel.ScanDirectories.Where(x => x != selected);

            _configuration.Directories.GameScanDirectories = updated;
            await _configuration.WriteConfiguration(_configuration.Directories).ConfigureAwait(true);

            _viewModel.ScanDirectories = _configuration.Directories.GameScanDirectories.ToList();
        }
    }
}