using System;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using SpectabisLib.Models;
using SpectabisUI.Interfaces;
using SpectabisUI.ViewModels;

namespace SpectabisUI.Controls.GameTileView
{
    public class GameTileView : UserControl, IGameTile
    {
        public GameProfile Profile { get; set; }
        public Image BoxArt { get; set; }

        private GameTileViewModel ViewModel { get; }

        [Obsolete("XAMLIL placeholder", true)]
        public GameTileView() { }

        public GameTileView(GameProfile game, GameTileViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;

            Profile = game;

            InitializeComponent();
            ReferenceChildren();

            PointerEnter += OnMousePointerEnter;
            PointerLeave += OnMousePointerLeave;
        }

        public void SetVisualTitle(string newTitle)
        {
            ViewModel.Title = newTitle;
        }

        public void LoadBoxart(Bitmap source)
        {
            BoxArt.Source = source;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ReferenceChildren()
        {
            BoxArt = this.FindControl<Image>("BoxArtImage");
        }

        private void OnMousePointerLeave(object sender, PointerEventArgs e)
        {
            ViewModel.ShowActiveEffect = false;
        }

        private void OnMousePointerEnter(object sender, PointerEventArgs e)
        {
            ViewModel.ShowActiveEffect = true;
        }
    }
}