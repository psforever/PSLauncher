using Newtonsoft.Json;
using System.Collections.Generic;

namespace Launcher.GameCheck
{
    [JsonObject]
    public class FileManifest
    {
        [JsonProperty(PropertyName = "FilesList")]
        public List<File> ListOfFiles { get; set; }
    }
}
