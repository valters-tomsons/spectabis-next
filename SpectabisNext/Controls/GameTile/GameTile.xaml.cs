using System;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Portable.Xaml.Markup;
using SpectabisLib.Models;
using SpectabisNext.Interfaces;

namespace SpectabisNext.Controls.GameTile
{
    public class GameTileView : UserControl, IGameTile
    {
        public GameProfile Profile { get; set; }
        private Image BoxArt { get; set; }
        private TextBlock BoxTitle { get; set; }
        private Rectangle HoverOverlayRectangle { get; set; }

        public GameTileView(GameProfile game)
        {
            InitializeComponent();
            ReferenceChildren();
            InitializeState(game);
            RegisterEvents();
        }

        public bool ShowHoverOverlay
        {
            get
            {
                return HoverOverlayRectangle.IsVisible;
            }
            set
            {
                HoverOverlayRectangle.IsVisible = value;
                BoxTitle.IsVisible = value;
            }
        }

        public void SetVisualTitle(string newTitle)
        {
            BoxTitle.Text = newTitle;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ReferenceChildren()
        {
            BoxArt = this.FindControl<Image>("BoxArtImage");
            BoxTitle = this.FindControl<TextBlock>("BoxTitle");
            HoverOverlayRectangle = this.FindControl<Rectangle>("HoverOverlay");
        }

        private void InitializeState(GameProfile game)
        {
            Profile = game;
            BoxArt.Source = ImageBitmapFromPath(game.BoxArtPath);
            SetVisualTitle(game.Title);
        }

        private void RegisterEvents()
        {
            this.PointerEnter += OnMousePointerEnter;
            this.PointerLeave += OnMousePointerLeave;
        }

        private void OnMousePointerLeave(object sender, PointerEventArgs e)
        {
            ShowHoverOverlay = false;
        }

        private void OnMousePointerEnter(object sender, PointerEventArgs e)
        {
            ShowHoverOverlay = true;
        }

        private Bitmap ImageBitmapFromPath(string path)
        {
            return new Bitmap(path);
        }
    }
}