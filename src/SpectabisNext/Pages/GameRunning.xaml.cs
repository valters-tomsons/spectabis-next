using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class GameRunning : UserControl, Page
    {
        private string pageTitle = "PCSX2";
        private bool showInTitlebar = false;
        private bool hideTitlebar = false;
        private bool reloadOnNavigation = true;

        public string PageTitle { get { return pageTitle; } }
        public bool ShowInTitlebar { get { return showInTitlebar; } }
        public bool HideTitlebar { get { return hideTitlebar; } }
        public bool ReloadOnNavigation { get { return reloadOnNavigation; } }

        public GameRunning()
        {
            InitializeComponent();
        }
        
        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}