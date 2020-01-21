using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisLib.Repositories;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class CreateProfile : UserControl, Page
    {
        private readonly GameProfileRepository _gameRepo;

        public string PageTitle { get; } = "Add Game";
        public bool ShowInTitlebar { get; } = true;
        public bool HideTitlebar { get; } = false;
        public bool ReloadOnNavigation { get; } = false;

        [Obsolete("XAMLIL placeholder", true)]
        public CreateProfile() { }

        public CreateProfile(GameProfileRepository gameRepo)
        {
            _gameRepo = gameRepo;

            InitializeComponent();
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}