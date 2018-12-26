using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SpectabisNext.Interfaces;
using SpectabisNext.Models.Configuration;

namespace SpectabisNext.Views
{
    public class MainWindow : Window
    {
        private readonly UIConfiguration _uiConfiguration;
        private readonly GameLibrary _gameLibrary;

        public MainWindow(UIConfiguration uiConfiguration, GameLibrary gameLibrary)
        {
            _gameLibrary = gameLibrary;
            _uiConfiguration = uiConfiguration;

            InitializeComponent();
            FillBackgroundColor();

            var container = this.FindControl<ContentControl>("ContentContainer");
            container.Content = _gameLibrary;
        }

        private void FillBackgroundColor()
        {
            this.Background = _uiConfiguration.UIBackgroundGradient;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}