using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisNext.ViewModels
{
    public class GameLibraryViewModel
    {
        public GameLibraryViewModel(IProfileRepository profileRepository)
        {
            var profiles = profileRepository.GetAll();
            ProfileCollection = profiles.Result;
        }

        public IEnumerable<GameProfile> ProfileCollection { get; set; }
    }
}