using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SpectabisLib.Services;
using SpectabisNext.Controls.GameTile;
using SpectabisNext.Interfaces;

namespace SpectabisNext.Views
{
    public class MainWindow : Window
    {
        private readonly GameProfileRepository _gameRepository;

        public MainWindow(GameProfileRepository gameRepo)
        {
            this._gameRepository = gameRepo;

            InitializeComponent();
            Populate();
            FillBackgroundColor();
        }

        private void FillBackgroundColor()
        {
            var bgBrush = new LinearGradientBrush()
            {
                StartPoint = new RelativePoint(0, 1, 0),
                EndPoint = new RelativePoint(0.5, 0, 0)
            };
            
            var stop1 = new GradientStop()
            {
                Color = Color.Parse("#BDBDBD"),
                Offset = 0
            };

            var stop2 = new GradientStop()
            {
                Color = Color.Parse("#F5F5F5"),
                Offset = 0.56
            };

            bgBrush.GradientStops.Add(stop1);
            bgBrush.GradientStops.Add(stop2);

            this.Background = bgBrush;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Populate()
        {
            var gamePanel = this.FindControl<WrapPanel>("GamePanel");
            var gg = new GameTileView(_gameRepository.GetAll().First());
            var gg2 = new GameTileView(_gameRepository.GetAll().First());
            gamePanel.Children.Add(gg);

            gg.PointerEnter += GamePointerEnter;
            gg.PointerLeave += GamePointerLeave;
        }

        private void GamePointerLeave(object sender, PointerEventArgs e)
        {
            var obj = (IGameTile) sender;
            obj.ShowHoverOverlay = false;
        }

        private void GamePointerEnter(object sender, PointerEventArgs e)
        {
            var obj = (IGameTile) sender;
            obj.ShowHoverOverlay = true;
        }
    }
}