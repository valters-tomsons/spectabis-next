using Avalonia.Media.Imaging;

namespace SpectabisNext.Interfaces
{
    public interface IBitmapLoader
    {
        Bitmap DefaultBoxart { get; }

        Bitmap LoadFromFile(string filePath);
    }
}