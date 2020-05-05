using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IDiscordService
    {
        void InitializeDiscord();
        void SetMenuPresence();
        void SetGamePresence(GameProfile game);
    }
}