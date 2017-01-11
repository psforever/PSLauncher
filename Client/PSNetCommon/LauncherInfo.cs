using Newtonsoft.Json;

namespace PSNetCommon
{
    /// <summary>
    /// Used for manifest.json
    /// </summary>
    [JsonObject]
    public class LauncherInfo
    {
        [JsonProperty("networkInfo")]
        public NetworkInfo NetInfo { get; set; }
        [JsonProperty("emulatorInfo")]
        public EmulatorInfo EmuInfo { get; set; }
    }
}
