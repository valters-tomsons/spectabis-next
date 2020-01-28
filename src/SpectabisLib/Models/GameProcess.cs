using System;
using System.Diagnostics;

namespace SpectabisLib.Models
{
    public class GameProcess
    {
        public delegate void GameStoppedEventHandler(object sender, EventArgs args);
        public event EventHandler<EventArgs> GameStopped;

        public GameProfile Game { get; }
        public Process Process { get; }

        public GameProcess(GameProfile game, Process process)
        {
            Game = game;
            Process = process;
        }

        public void Start()
        {
            Process.Exited += OnGameProcessExited;
            Process.Start();
        }

        public void Stop()
        {
            Process.Kill();
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