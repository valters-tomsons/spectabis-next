using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SpectabisNext.Interfaces;
using SpectabisNext.Models.Configuration;
using SpectabisNext.Repositories;

namespace SpectabisNext.Views
{
    public class MainWindow : Window
    {
        private readonly UIConfiguration _uiConfiguration;
        private readonly PageRepository _pageRepository;

        public MainWindow(UIConfiguration uiConfiguration, PageRepository pageRepository)
        {
            _uiConfiguration = uiConfiguration;
            _pageRepository = pageRepository;

            InitializeComponent();
            FillBackgroundColor();

            var container = this.FindControl<ContentControl>("ContentContainer");
            container.Content = _pageRepository.GetAll().First();
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