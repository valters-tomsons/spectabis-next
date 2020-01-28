using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class Settings : UserControl, IPage
    {
        public string PageTitle => "Settings";
        public bool ShowInTitlebar => true;
        public bool HideTitlebar => false;
        public bool ReloadOnNavigation => false;

        public Settings()
        {
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}