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
        private readonly ProfileFileSystem _profileFs;

        public BackgroundQueueService(ISpectabisClient client, ProfileFileSystem profileFs)
        {
            _client = client;
            _profileFs = profileFs;

            _gameArtQueue = new Queue<GameProfile>();
            _gameArtTasks = new List<Task<GameProfile>>();

            InitializeArtThread();
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

            Console.WriteLine($"[QueueService] Downloading boxart");
            var boxBytes = await _client.DownloadBoxArt(game.SerialNumber).ConfigureAwait(false);

            Console.WriteLine($"[QueueService] Writing boxart to file system");
            await _profileFs.WriteGameBoxArtImage(game, boxBytes).ConfigureAwait(false);
        }
    }
}