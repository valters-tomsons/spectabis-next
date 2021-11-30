using System;
using System.Collections.Generic;
using System.ComponentModel;
using ServiceLib.Interfaces;
using Common.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Interfaces.Abstractions;
using SpectabisLib.Interfaces.Services;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameArtQueue : IArtServiceQueue
    {
        private BackgroundWorker? _gameArtThread;
        private readonly Queue<GameProfile> _gameArtQueue;
        private readonly Stack<GameProfile> _finishedArt;

        private readonly ISpectabisClient _client;
        private readonly IProfileFileSystem _profileFs;
        private readonly ILocalCachingService _localCache;

        private GameProfile? _currentProcess;

        public event EventHandler<EventArgs>? ItemFinished;

        public GameArtQueue(ISpectabisClient client, IProfileFileSystem profileFs, ILocalCachingService localCache)
        {
            _client = client;
            _profileFs = profileFs;
            _localCache = localCache;

            _gameArtQueue = new Queue<GameProfile>();
            _finishedArt = new Stack<GameProfile>();

            InitializeArtThread();
        }

        public void StartProcessing()
        {
            _gameArtThread?.RunWorkerAsync();
        }

        public void QueueForBoxArt(GameProfile game)
        {
            var gameQueued = _gameArtQueue.Contains(game);

            if (gameQueued || string.IsNullOrWhiteSpace(game.SerialNumber))
            {
                Logging.WriteLine($"Not queuing '{game.Id}' because serial is null");
                return;
            }

            _gameArtQueue.Enqueue(game);
        }

        public GameProfile PopFinishedGames()
        {
            return _finishedArt.Pop();
        }

        public bool IsProcessing(GameProfile game)
        {
            return _gameArtQueue.Contains(game) || _finishedArt.Contains(game) || _currentProcess == game;
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
                Logging.WriteLine("Queue is empty");
                return;
            }

            var game = _gameArtQueue.Dequeue();
            _currentProcess = game;
            Logging.WriteLine($"Dequeued '{game.Id}'");

            var boxBytes = await _localCache.GetCachedArt(game.SerialNumber).ConfigureAwait(false);

            if(boxBytes == null)
            {
                Logging.WriteLine("Downloading boxart");

                boxBytes = await _client.DownloadBoxArt(game.SerialNumber).ConfigureAwait(false);
                await _localCache.WriteArtToCache(game.SerialNumber, boxBytes).ConfigureAwait(false);
            }

            if (boxBytes != null)
            {
                Logging.WriteLine("Writing boxart to file system");
                await _profileFs.WriteProfileArtToFileSystem(game, boxBytes).ConfigureAwait(false);
            }
            else
            {
                Logging.WriteLine("Boxart download failed");
            }

            _finishedArt.Push(game);
            _currentProcess = null;

            OnItemFinished();
        }
    }
}