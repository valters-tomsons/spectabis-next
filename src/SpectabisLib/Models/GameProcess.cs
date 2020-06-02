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

        private readonly Stopwatch playTimer = new Stopwatch();

        public GameProcess(GameProfile game, Process process)
        {
            Game = game;
            Process = process;
        }

        public void Start()
        {
            Process.Exited += OnGameProcessExited;
            Process.Start();
            playTimer.Start();
        }

        public TimeSpan Stop()
        {
            playTimer.Stop();
            Process.Kill();
            OnGameStopped();

            Process.Exited -= OnGameProcessExited;

            return GetSessionLength();
        }

        public TimeSpan GetSessionLength()
        {
            return playTimer.Elapsed;
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