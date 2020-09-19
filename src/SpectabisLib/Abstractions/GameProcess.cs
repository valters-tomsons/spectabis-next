using System;
using System.Diagnostics;
using SpectabisLib.Models;

namespace SpectabisLib.Abstractions
{
    public class GameProcess
    {
        public delegate void GameStoppedEventHandler(object sender, EventArgs args);
        public event EventHandler<EventArgs> GameStopped;

        public GameProfile Game { get; }
        public Process Process { get; }
        public Stopwatch Stopwatch { get; }

        public GameProcess(GameProfile game, Process process)
        {
            Stopwatch = new Stopwatch();

            Game = game;
            Process = process;
        }

        public void Start()
        {
            Process.Exited += OnGameProcessExited;
            Process.Start();
            Stopwatch.Start();
        }

        public TimeSpan Stop()
        {
            Stopwatch.Stop();
            Process.Kill();
            OnGameStopped();

            Process.Exited -= OnGameProcessExited;

            return GetElapsed();
        }

        public TimeSpan GetElapsed()
        {
            return Stopwatch.Elapsed;
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