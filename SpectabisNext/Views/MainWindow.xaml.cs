using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
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
        private Rectangle Titlebar;
        private ContentControl ContentContainer;

        public MainWindow(UIConfiguration uiConfiguration, PageRepository pageRepository)
        {
            _uiConfiguration = uiConfiguration;
            _pageRepository = pageRepository;

            InitializeComponent();
            RegisterChildern();
            FillElementColors();

            ContentContainer.Content = _pageRepository.GetAll().First();
        }

        private void FillElementColors()
        {
            Background = _uiConfiguration.UIBackgroundGradient;
            Titlebar.Fill = _uiConfiguration.TitlebarGradient;
        }

        private void RegisterChildern()
        {
            Titlebar = this.FindControl<Rectangle>("Titlebar");
            ContentContainer = this.FindControl<ContentControl>("ContentContainer");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}