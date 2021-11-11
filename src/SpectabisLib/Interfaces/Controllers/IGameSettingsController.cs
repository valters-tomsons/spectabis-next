using System;
using System.Threading.Tasks;

namespace SpectabisLib.Interfaces.Controllers
{
    public interface IGameSettingsController
    {
        Task LaunchConfiguration(Guid gameId);
    }
}