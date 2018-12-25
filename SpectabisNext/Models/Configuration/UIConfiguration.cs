using Avalonia.Media;

namespace SpectabisNext.Models.Configuration
{
    public class UIConfiguration
    {
        public UIConfiguration()
        {
            BoxArtWidth = 150;
            BoxArtHeight = 200;
            BoxArtSizeModifier = 1.2;

            UIBackgroundGradient = new LinearGradientBrush()
            {
                StartPoint = new Avalonia.RelativePoint(0.5, 1, 0),
                EndPoint = new Avalonia.RelativePoint(0.5, 0, 0),

                GradientStops = {
                new GradientStop()
                {
                Color = Color.Parse("#BDBDBD"),
                Offset = 0
                },

                new GradientStop()
                {
                Color = Color.Parse("#F5F5F5"),
                Offset = 0.56
                }
                }
            };
        }

        public double BoxArtWidth { get; set; }
        public double BoxArtHeight { get; set; }
        public double BoxArtSizeModifier { get; set; }
        public LinearGradientBrush UIBackgroundGradient { get; set; }
    }
}