using Avalonia.Media.Imaging;
using ReactiveUI;
using SpectabisLib.Interfaces;
using SpectabisUI.Interfaces;

namespace SpectabisNext.ViewModels
{
    public class CreateProfileViewModel : ReactiveObject
    {
        private readonly IBitmapLoader _bitmapLoader;
        private readonly IConfigurationLoader _configuration;

        private string serialNumber;
        private bool serialEnabled = true;
        private Bitmap boxArtImage;
        private double boxArtWidth;
        private double boxArtHeight;
        private string gameTitle;

        public CreateProfileViewModel(IBitmapLoader bitmapLoader, IConfigurationLoader configuration)
        {
            _configuration = configuration;
            _bitmapLoader = bitmapLoader;

            boxArtImage = _bitmapLoader.DefaultBoxart;
            boxArtWidth = _configuration.UserInterface.BoxArtWidth;
            boxArtHeight = _configuration.UserInterface.BoxArtHeight;
        }

        public string SerialNumber { get => serialNumber; set => this.RaiseAndSetIfChanged(ref serialNumber, value); }
        public bool SerialEnabled { get => serialEnabled; set => this.RaiseAndSetIfChanged(ref serialEnabled, value); }
        public Bitmap BoxArtImage { get => boxArtImage; set => this.RaiseAndSetIfChanged(ref boxArtImage, value); }
        public double BoxArtWidth { get => boxArtWidth; set => this.RaiseAndSetIfChanged(ref boxArtWidth, value); }
        public double BoxArtHeight { get => boxArtHeight; set => this.RaiseAndSetIfChanged(ref boxArtHeight, value); }
        public string GameTitle { get => gameTitle; set => this.RaiseAndSetIfChanged(ref gameTitle, value); }
    }
}