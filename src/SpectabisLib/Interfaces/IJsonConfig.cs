using Newtonsoft.Json;

namespace SpectabisLib.Interfaces
{
    public interface IJsonConfig
    {
        [JsonIgnore]
        string FileName { get; }
    }
}