using Avalonia.Media;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Configuration
{
    public class UIConfig : IJsonConfig
    {
        public string FileName => nameof(UIConfig).ConfigClassToFileName();

        public UIConfig()
        {
            BoxArtWidth = 150;
            BoxArtHeight = 200;
            BoxArtSizeModifier = 1.2;
            BoxArtGapSize = 10;

            UIBackgroundGradient = DefaultBackgroundGradient();
            TitlebarGradient = DefaultTitlebarGradient();
        }

        public double BoxArtWidth { get; set; }
        public double BoxArtHeight { get; set; }
        public double BoxArtSizeModifier { get; set; }
        public double BoxArtGapSize { get; set; }
        public LinearGradientBrush UIBackgroundGradient { get; set; }
        public LinearGradientBrush TitlebarGradient { get; set; }

        private LinearGradientBrush DefaultBackgroundGradient()
        {
            var gradient = new LinearGradientBrush()
            {
                StartPoint = new Avalonia.RelativePoint(0.5, 1, 0),
                EndPoint = new Avalonia.RelativePoint(0.5, 0, 0),
            };

            var stop1 = new GradientStop()
            {
                Color = Color.Parse("#BDBDBD"),
                Offset = 0
            };

            var stop2 = new GradientStop()
            {
                Color = Color.Parse("#F5F5F5"),
                Offset = 0.56
            };

            gradient.GradientStops.Add(stop1);
            gradient.GradientStops.Add(stop2);

            return gradient;
        }

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