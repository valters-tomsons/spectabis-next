using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SpectabisNext.Views
{
    public class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();

            var text = this.FindControl<TextBlock>("MyText");
            text.Text = $"Settings: {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

}