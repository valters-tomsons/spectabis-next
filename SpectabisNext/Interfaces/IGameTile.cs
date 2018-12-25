using SpectabisLib.Models;

namespace SpectabisNext.Interfaces
{
    public interface IGameTile
    {
        GameProfile Profile { get; set; }
        Avalonia.Controls.Controls Children { get; }
        void SetVisualTitle(string newTitle);
        bool ShowHoverOverlay { get; set; }
    }
}