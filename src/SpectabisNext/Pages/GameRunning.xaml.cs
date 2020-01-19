using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class GameRunning : UserControl, Page
    {
        public string PageTitle { get; } = "PCSX2";
        public bool ShowInTitlebar { get; } = false;
        public bool HideTitlebar { get; } = false;
        public bool ReloadOnNavigation { get; } = true;

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