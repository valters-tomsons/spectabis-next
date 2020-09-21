using Newtonsoft.Json;

namespace SpectabisLib.Interfaces
{
    public interface IJsonConfig
    {
        [JsonIgnore]
        string ConfigName { get; }
    }
}