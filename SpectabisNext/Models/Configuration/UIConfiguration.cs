namespace SpectabisNext.Models.Configuration
{
    public class UIConfiguration
    {
        public UIConfiguration()
        {
            BoxArtWidth = 150;
            BoxArtHeight = 200;
            BoxArtSizeModifier = 1.2;
        }

        public double BoxArtWidth { get; set; }
        public double BoxArtHeight { get; set; }
        public double BoxArtSizeModifier { get; set; }
    }
}