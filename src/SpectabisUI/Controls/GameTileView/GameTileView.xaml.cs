using System;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using SpectabisLib.Models;
using SpectabisUI.Interfaces;

namespace SpectabisUI.Controls.GameTileView
{
    public class GameTileView : UserControl, IGameTile
    {
        public GameProfile Profile { get; set; }
        public Image BoxArt { get; set; }
        private TextBlock BoxTitle { get; set; }
        private Rectangle HoverOverlayRectangle { get; set; }

        [Obsolete("XAMLIL placeholder", true)]
        public GameTileView() { }

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
            BoxTitle = this.FindControl<TextBlock>("BoxTitle");
            HoverOverlayRectangle = this.FindControl<Rectangle>("HoverOverlay");
        }

        private void InitializeState(GameProfile game)
        {
            Profile = game;
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
    }
}