using System.Collections.Generic;
using System.Collections.ObjectModel;
using SpectabisLib.Models;

namespace SpectabisLib.Repositories
{
    public class GameProfileRepository
    {
        private List<GameProfile> Games { get; set; }

        public GameProfileRepository()
        {
            Games = new List<GameProfile>();

            var game1 = new GameProfile()
            {
                Title = "Shadow of the Colossus",
                BoxArtPath = $"/{SystemDirectories.HomePath}/Downloads/sotc.jpg"
            };

            Games.Add(game1);
        }

        public ReadOnlyCollection<GameProfile> GetAll()
        {
            return Games.AsReadOnly();
        }
    }
}