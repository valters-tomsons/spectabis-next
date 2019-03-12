using Avalonia.Media.Imaging;
using SpectabisLib.Models;

namespace SpectabisNext.Interfaces
{
    public interface IGameTile
    {
        GameProfile Profile { get; set; }
        void SetVisualTitle(string newTitle);
        bool ShowHoverOverlay { get; set; }
        void LoadBoxart(Bitmap source);
    }
}