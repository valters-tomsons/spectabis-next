using System.Collections.Generic;
using System.Collections.ObjectModel;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Repositories
{
    public class GameProfileRepository : IProfileRepository
    {
        private List<GameProfile> Games { get; set; }

        public GameProfileRepository()
        {
            Games = new List<GameProfile>();

            for (int i = 0; i < 30; i++)
            {
                var game = new GameProfile()
                {
                    Title = "Placeholder Game",
                };

                Games.Add(game);
            }
        }

        public IReadOnlyCollection<GameProfile> GetAll()
        {
            return Games.AsReadOnly();
        }

        public void Add(GameProfile profile)
        {
            Games.Add(profile);
        }
    }
}