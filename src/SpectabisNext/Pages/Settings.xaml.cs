using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class Settings : UserControl, Page
    {
        private string pageTitle = "Settings";
        private bool showInTitlebar = true;
        private bool hideTitlebar = false;
        private bool reloadOnNavigation = false;

        public string PageTitle { get { return pageTitle; } }
        public bool ShowInTitlebar { get { return showInTitlebar; } }
        public bool HideTitlebar { get { return hideTitlebar; } }
        public bool ReloadOnNavigation { get { return reloadOnNavigation; } }

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