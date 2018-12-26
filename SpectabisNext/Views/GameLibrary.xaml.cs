using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Portable.Xaml.Markup;
using SpectabisLib.Repositories;
using SpectabisNext.Controls;
using SpectabisNext.Factories;
using SpectabisNext.Interfaces;

namespace SpectabisNext.Views
{
    public class GameLibrary : Page
    {
        private readonly GameProfileRepository _gameRepo;
        private readonly GameTileFactory _tileFactory;
        public GameLibrary(GameProfileRepository gameRepo, GameTileFactory tileFactory)
        {
            _tileFactory = tileFactory;
            _gameRepo = gameRepo;

            this.PageTitle = "Library";
            Populate();
        }

        private void Populate()
        {
            var gamePanel = this.FindControl<WrapPanel>("GamePanel");
            var game = _gameRepo.GetAll().First();

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