using EmuConfig.Enums;
using EmuConfig.Attributes;
using EmuConfig.Abstractions;

namespace EmuConfig.Configs
{
    public class GSdxConfig : IniConfiguration
    {
        [IniKey("upscale_multiplier")]
        public UpscaleFactor UpscaleFactor { get; set; }

        [IniKey("MaxAnisotropy")]
        public int AnisotropicFiltering { get; set; }

        [IniKey("Renderer")]
        public GSRendererType Renderer { get; set; }
    }
}