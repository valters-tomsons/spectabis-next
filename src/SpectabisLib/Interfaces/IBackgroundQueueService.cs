using System;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IBackgroundQueueService
    {
        void QueueForBoxArt(GameProfile game);
        void StartProcessing();
        GameProfile PopFinishedGames();
        bool IsProcessing(GameProfile game);
        event EventHandler<EventArgs> ItemFinished;
    }
}