using System;
using System.Diagnostics;
using SpectabisLib.Interfaces.Abstractions;
using SpectabisLib.Models;

namespace SpectabisLib.Abstractions
{
    public class GameProcess : IGameProcess
    {
        public event EventHandler<EventArgs> GameStopped;

        private GameProfile Game { get; }
        private Process Process { get; }
        private Stopwatch Watch { get; }

        public GameProcess(GameProfile game, Process process)
        {
            Watch = new Stopwatch();

            Game = game;
            Process = process;
        }

        public void Start()
        {
            Process.Exited += OnGameProcessExited;
            Process.Start();
            Watch.Start();
        }

        public void Stop()
        {
            Watch.Stop();
            Process.Kill();
            OnGameStopped();

            Process.Exited -= OnGameProcessExited;
        }

        public TimeSpan GetElapsed()
        {
            return Watch.Elapsed;
        }

        public GameProfile GetGame()
        {
            return Game;
        }

        public Process GetProcess()
        {
            return Process;
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