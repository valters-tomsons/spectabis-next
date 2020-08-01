using Avalonia.Controls;
using SpectabisLib.Models;

namespace SpectabisUI.Interfaces
{
    public interface IGifProvider
    {
        void StartSpinner(GameProfile game, Image tile);
        void DisponseSpinner(GameProfile game);
    }
}