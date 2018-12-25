using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Portable.Xaml.Markup;
using SpectabisLib.Models;
using SpectabisNext.Interfaces;

namespace SpectabisNext.Controls.GameTile
{
    [ContentProperty(nameof(Children))]
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

            Profile = game;
            BoxArt.Source = ImageBitmapFromPath(Profile.BoxArtPath);
            SetVisualTitle(Profile.Title);
        }

        public Avalonia.Controls.Controls Children
        {
            get
            {
                var controlGrid = this.FindControl<Grid>("TileControlGrid");
                return controlGrid.Children;
            }
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

        private Bitmap ImageBitmapFromPath(string path)
        {
            return new Bitmap(path);
        }
    }
}