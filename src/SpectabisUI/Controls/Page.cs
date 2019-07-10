using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SpectabisUI.Controls
{
    public class Page : UserControl
    {
        public string PageTitle { get; set; }
        public bool HideTitlebar { get; set; }
        public bool ShowInTitlebar { get; set; }
        public bool ReloadOnNavigation { get; set; }

        public Page()
        {
            InitializeComponent();

            HideTitlebar = false;
            ShowInTitlebar = false;
            ReloadOnNavigation = false;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}