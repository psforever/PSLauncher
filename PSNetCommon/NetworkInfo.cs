using System.Collections.Generic;
using Newtonsoft.Json;

namespace PSNetCommon
{
    /// <summary>
    /// Provides information about the available login servers
    /// and service that hosts servers.
    /// </summary>
    [JsonObject(id: "network")]
    public class NetworkInfo
    {
        public NetworkInfo()
        {
            Servers = new List<Server>();
        }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("servers")]
        public List<Server> Servers { get; set; }
    }

    /// <summary>
    /// Provides information about a specific login server.
    /// </summary>
    [JsonObject(id: "server")]
    public class Server
    {
        [JsonProperty("namespace")]
        public string Namespace { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("port")]
        public string Port { get; set; }
    }
}
