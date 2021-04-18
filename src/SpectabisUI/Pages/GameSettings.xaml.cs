using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisUI.Interfaces;

namespace SpectabisUI.Pages
{
    public class GameSettings : UserControl, IPage
    {
        public string PageTitle => "PCSX2";
        public bool ShowInTitlebar => false;
        public bool HideTitlebar => false;
        public bool ReloadOnNavigation => true;

        // [Obsolete("XAMLIL placeholder", true)]
        public GameSettings()
        {
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}