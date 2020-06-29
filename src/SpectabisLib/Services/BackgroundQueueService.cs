using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ServiceClient.Interfaces;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class BackgroundQueueService : IBackgroundQueueService
    {
        private BackgroundWorker _gameArtThread;
        private readonly Queue<GameProfile> _gameArtQueue;
        private readonly Stack<GameProfile> _finishedArt;

        private readonly ISpectabisClient _client;
        private readonly ProfileFileSystem _profileFs;

        public event EventHandler<EventArgs> ItemFinished;

        public BackgroundQueueService(ISpectabisClient client, ProfileFileSystem profileFs)
        {
            _client = client;
            _profileFs = profileFs;

            _gameArtQueue = new Queue<GameProfile>();
            _finishedArt = new Stack<GameProfile>();

            InitializeArtThread();
        }

        public void StartProcessing()
        {
            _gameArtThread.RunWorkerAsync();
        }

        public void QueueForBoxArt(GameProfile game)
        {
            var gameQueued = _gameArtQueue.Contains(game);

            if (gameQueued || string.IsNullOrWhiteSpace(game.SerialNumber))
            {
                Console.WriteLine($"[QueueService] Not queuing '{game.Id}' because serial is null");
                return;
            }

            _gameArtQueue.Enqueue(game);
        }

        public GameProfile GetLastFinishedGame()
        {
            return _finishedArt.Pop();
        }

        protected virtual void OnItemFinished()
        {
            ItemFinished?.Invoke(this, EventArgs.Empty);
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
            if (_gameArtQueue.Count == 0)
            {
                Console.WriteLine("[QueueService] Queue is empty");
                return;
            }

            var game = _gameArtQueue.Dequeue();
            Console.WriteLine($"[QueueService] Dequeued '{game.Id}'");

            Console.WriteLine("[QueueService] Downloading boxart");
            var boxBytes = await _client.DownloadBoxArt(game.SerialNumber).ConfigureAwait(false);

            Console.WriteLine($"[QueueService] Writing boxart to file system");
            await _profileFs.WriteGameBoxArtImage(game, boxBytes).ConfigureAwait(false);

            _finishedArt.Push(game);
            OnItemFinished();
        }
    }
}