using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SpectabisNext.Controls
{
    public class Page : UserControl
    {
        public string PageTitle { get; set; }

        public Page()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}