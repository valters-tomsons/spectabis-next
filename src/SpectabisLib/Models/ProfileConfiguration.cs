using System;
using EmuConfig.Configs;

namespace SpectabisLib.Models
{
    public class ProfileConfiguration
    {
        public Guid Id { get; set; }
        public GSdxConfig GSdxConfig { get; set; }
    }
}