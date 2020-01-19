using System.Diagnostics;

namespace SpectabisLib.Models
{
    public class GameProcess
    {
        private readonly Process _process;

        private readonly GameProfile _game;

        public GameProfile Game { get => _game; }
        public Process Process { get => _process; }

        public GameProcess(GameProfile game, Process process)
        {
            _game = game;
            _process = process;
        }

        public void Start()
        {
            _process.Start();
        }

        public void Stop()
        {
            _process.Kill();
        }
    }
}