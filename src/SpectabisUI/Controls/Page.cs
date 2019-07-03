using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SpectabisUI.Controls
{
    public class Page : UserControl
    {
        public string PageTitle { get; set; }
        public bool HideTitlebar { get; set; }
        public bool ShowInTitlebar { get; set; }

        public Page()
        {
            InitializeComponent();

            HideTitlebar = false;
            ShowInTitlebar = false;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}