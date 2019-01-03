using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SpectabisNext.Controls
{
    public class Page : UserControl
    {
        public string PageTitle { get; set; }
        public bool HideTitlebar { get; set; }

        public Page()
        {
            InitializeComponent();
            HideTitlebar = false;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}