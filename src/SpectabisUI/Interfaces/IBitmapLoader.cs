using Avalonia.Media.Imaging;
using SpectabisLib.Models;

namespace SpectabisUI.Interfaces
{
    public interface IBitmapLoader
    {
        Bitmap DefaultBoxart { get; }
        Bitmap GetBoxArt(GameProfile game);
    }
}