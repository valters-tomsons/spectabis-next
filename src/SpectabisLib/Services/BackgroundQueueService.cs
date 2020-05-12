using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ServiceClient.Interfaces;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class BackgroundQueueService : IBackgroundQueueService
    {
        private BackgroundWorker _gameArtThread;
        private Queue<GameProfile> _gameArtQueue;
        private IEnumerable<Task<GameProfile>> _gameArtTasks;

        private readonly ISpectabisClient _client;

        public BackgroundQueueService(ISpectabisClient client)
        {
            _client = client;
            InitializeArtThread();
            _gameArtQueue = new Queue<GameProfile>();
            _gameArtTasks = new List<Task<GameProfile>>();
        }

        public void StartProcessing()
        {
            _gameArtThread.RunWorkerAsync();
        }

        public void QueueForBoxArt(GameProfile game)
        {
            var gameQueued = _gameArtQueue.Contains(game);

            if (!gameQueued)
            {
                _gameArtQueue.Enqueue(game);
            }
        }

        private void InitializeArtThread()
        {
            _gameArtThread = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            _gameArtThread.DoWork += DownloadArtLatestInQueue;
        }

        private async void DownloadArtLatestInQueue(object sender, DoWorkEventArgs e)
        {
            var game = _gameArtQueue.Dequeue();
            Console.WriteLine($"[QueueService] Dequeued '{game.Id}'");

            var boxArtUrl = await _client.GetBoxArtUrl(game.SerialNumber).ConfigureAwait(false);
            Console.WriteLine($"[QueueService] Url Retrieved: {boxArtUrl}");
        }
    }
}