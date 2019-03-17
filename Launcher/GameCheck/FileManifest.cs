using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Launcher.GameCheck
{
    [JsonObject]
    public class FileManifest
    {
        [JsonProperty(PropertyName = "Host")]
        public Uri Host { get; set; }
        [JsonProperty(PropertyName = "FilesList")]
        public List<File> ListOfFiles { get; set; }
    }
}
