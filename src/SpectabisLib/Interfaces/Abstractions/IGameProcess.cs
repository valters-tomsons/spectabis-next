using System;
using System.Diagnostics;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces.Abstractions
{
    public interface IGameProcess
    {
        event EventHandler<EventArgs>? GameStopped;

        TimeSpan GetElapsed();
        GameProfile GetGame();
        Process GetProcess();
        void Start();
        void Stop();
    }
}