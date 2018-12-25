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
using SpectabisNext.Models.Configuration;

namespace SpectabisNext.Views
{
    public class MainWindow : Window
    {
        private readonly GameProfileRepository _gameRepository;
        private readonly GameTileFactory _tileFactory;
        private readonly UIConfiguration _uiConfiguration;

        public MainWindow(GameProfileRepository gameRepo, GameTileFactory tileFactory, UIConfiguration uiConfiguration)
        {
            _uiConfiguration = uiConfiguration;
            _gameRepository = gameRepo;
            _tileFactory = tileFactory;

            InitializeComponent();

            Populate();
            FillBackgroundColor();
        }

        private void FillBackgroundColor()
        {
            this.Background = _uiConfiguration.UIBackgroundGradient;
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