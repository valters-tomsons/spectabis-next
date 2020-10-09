using System;
using EmuConfig.Enums;
using EmuConfig.Attributes;
using EmuConfig.Bases;

namespace EmuConfig.Configs
{
    public class GSdxConfig : IniConfiguration
    {
        [IniKey("upscale_multiplier")]
        public UpscaleFactor UpscaleFactor { get; set; }
    }
}