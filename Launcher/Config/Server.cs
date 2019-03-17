using Newtonsoft.Json;
using System.Net;

namespace Launcher.Config
{
    [JsonObject]
    public class Server
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonRequired]
        [JsonProperty(PropertyName = "Address")]
        public IPAddress Address { get; set; }
        [JsonRequired]
        [JsonProperty(PropertyName = "Location")]
        public string ServerLocation { get; set; }
        [JsonRequired]
        [JsonProperty(PropertyName = "ServerType")]
        public ServerType ServerType { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "ManifestLocation")]
        public string Manifest { get; set; }
    }

    public enum ServerType
    {
        Indev, Alpha, Beta, Release
    }
}
