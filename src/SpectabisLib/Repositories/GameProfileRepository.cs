using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisLib.Services;

namespace SpectabisLib.Repositories
{
    public class GameProfileRepository : IProfileRepository
    {
        private List<GameProfile> Games { get; }
        private ProfileFileSystem _pfs { get; }

        public GameProfileRepository(ProfileFileSystem pfs)
        {
            _pfs = pfs;

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

        public async Task UpsertProfile(GameProfile profile)
        {
            if(profile.Id == Guid.Empty)
            {
                profile.Id = Guid.NewGuid();
            }

            await _pfs.WriteProfileAsync(profile).ConfigureAwait(false);
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