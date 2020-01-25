using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IProfileFactory
    {
        GameProfile CreateFromPath(string gameFilePath);
    }
}