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
    }
}
