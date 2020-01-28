using System.Collections.Generic;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisNext.ViewModels
{
    public class GameLibraryViewModel
    {
        public GameLibraryViewModel(IProfileRepository profileRepository)
        {
            ProfileCollection = profileRepository.GetAll();
        }

        public IEnumerable<GameProfile> ProfileCollection { get; set; }
    }
}