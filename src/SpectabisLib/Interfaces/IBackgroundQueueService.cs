using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IBackgroundQueueService
    {
        void QueueForBoxArt(GameProfile game);
        void StartProcessing();
    }
}