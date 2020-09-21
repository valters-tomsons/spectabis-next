using Newtonsoft.Json;

namespace SpectabisLib.Interfaces
{
    public interface IJsonConfig
    {
        [JsonIgnoreAttribute]
        string FileName { get; }
    }
}