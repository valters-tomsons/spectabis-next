using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SpectabisLib.Repositories;
using SpectabisNext.Controls.GameTile;
using SpectabisNext.Factories;
using SpectabisNext.Interfaces;

namespace SpectabisNext.Views
{
    public class MainWindow : Window
    {
        private readonly GameProfileRepository _gameRepository;
        private readonly GameTileFactory _tileFactory;

        public MainWindow(GameProfileRepository gameRepo, GameTileFactory tileFactory)
        {
            _gameRepository = gameRepo;
            _tileFactory = tileFactory;

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
            var game = _gameRepository.GetAll().First();

            var gg = _tileFactory.Create(game);
            var gg2 = _tileFactory.Create(game);

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