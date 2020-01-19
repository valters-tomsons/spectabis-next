using System;
using System.Diagnostics;

namespace SpectabisLib.Models
{
    public class GameProcess
    {
        private readonly Process _process;

        private readonly GameProfile _game;

        public delegate void GameStoppedEventHandler(object sender, EventArgs args);
        public event GameStoppedEventHandler GameStopped;

        public GameProfile Game { get => _game; }
        public Process Process { get => _process; }

        public GameProcess(GameProfile game, Process process)
        {
            _game = game;
            _process = process;
        }

        public void Start()
        {
            _process.Exited += OnGameProcessExited;
            _process.Start();
        }

        public void Stop()
        {
            _process.Kill();
            OnGameStopped();
        }

        private void OnGameProcessExited(object sender, EventArgs e)
        {
            OnGameStopped();
        }

        protected virtual void OnGameStopped()
        {
            GameStopped?.Invoke(this, EventArgs.Empty);
        }
    }
}