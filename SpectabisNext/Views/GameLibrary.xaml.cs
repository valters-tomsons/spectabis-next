using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Portable.Xaml.Markup;
using SpectabisLib.Repositories;
using SpectabisNext.Controls;
using SpectabisNext.Factories;
using SpectabisUI.Controls;

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
        
            foreach(var gameProfile in _gameRepo.GetAll())
            {
                var gameTile = _tileFactory.Create(gameProfile);
                gamePanel.Children.Add(gameTile);
            }
        }
    }
}