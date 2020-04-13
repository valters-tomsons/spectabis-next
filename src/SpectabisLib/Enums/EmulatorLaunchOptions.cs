using System;

namespace SpectabisLib.Enums
{
    [Flags]
    public enum EmulatorLaunchOptions
    {
        None = 0,
        Fullscreen = 1,
        Windowed = 2,
        Nogui = 4,
        Nodisc = 8,
        Nohacks = 16,
        Fullboot = 32,
    }
}