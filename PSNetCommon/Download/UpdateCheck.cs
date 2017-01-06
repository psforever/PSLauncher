using Newtonsoft.Json;

namespace PSNetCommon.Download
{
    [JsonObject(id: "update")]
    public class UpdateCheck
    {
        [JsonProperty("location")]
        public string UpdateLocation { get; set; }
        [JsonProperty("check")]
        public UpdateCheck Check { get; set; }
    }
}
