using Avalonia.Media.Imaging;

namespace SpectabisUI.Interfaces
{
    public interface IBitmapLoader
    {
        Bitmap DefaultBoxart { get; }

        Bitmap LoadFromFile(string filePath);
    }
}