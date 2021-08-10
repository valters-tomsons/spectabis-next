using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IDiscordService
    {
        void SetMenuPresence();
        void SetGamePresence(GameProfile game);
    }
}