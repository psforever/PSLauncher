using Newtonsoft.Json;
using System.Collections.Generic;

namespace Launcher.Config
{
    [JsonObject]
    public class ServerList
    {
        [JsonProperty(PropertyName = "Servers")]
        public List<Server> Servers { get; set; }

        public void Save()
        {

        }

        public void Load()
        {

        }
    }
}
