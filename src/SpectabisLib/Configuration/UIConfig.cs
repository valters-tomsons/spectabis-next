using Avalonia.Media;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Configuration
{
    public class UIConfig : IJsonConfig
    {
        public string ConfigName => nameof(UIConfig).ConfigClassToFileName();

        public UIConfig()
        {
            BoxArtWidth = 150;
            BoxArtHeight = 200;
            BoxArtSizeModifier = 1.25;
            BoxArtGapSize = 10;
            GameViewWidth = 400;

            TitlebarGradient = DefaultTitlebarGradient();
        }

        public double BoxArtWidth { get; set; }
        public double BoxArtHeight { get; set; }
        public double BoxArtSizeModifier { get; set; }
        public double BoxArtGapSize { get; set; }
        public LinearGradientBrush TitlebarGradient { get; set; }
        public double GameViewWidth { get; set; }

        private LinearGradientBrush DefaultTitlebarGradient()
        {
            var gradient = new LinearGradientBrush();

            var stop1 = new GradientStop()
            {
                Color = Color.Parse("#24C6DC"),
                Offset = 0
            };

            var stop2 = new GradientStop()
            {
                Color = Color.Parse("#514A9D"),
                Offset = 0.56789
            };

            gradient.GradientStops.Add(stop1);
            gradient.GradientStops.Add(stop2);

            return gradient;
        }
    }
}