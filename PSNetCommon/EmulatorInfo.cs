using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PSNetCommon
{
    /// <summary>
    /// Information used for the Launcher's Update Information
    /// </summary>
    [JsonObject(id: "emulator")]
    public class EmulatorInfo
    {
        public EmulatorInfo()
        {
            Patches = new List<Patch>();
        }

        [JsonProperty("patches")]
        public List<Patch> Patches { get; set; }
    }

    /// <summary>
    /// Patch used in Emulator Information
    /// </summary>
    [JsonObject(id: "patch")]
    public class Patch
    {
        public Patch()
        {
            Changes = new List<Change>();
        }

        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("changes")]
        public List<Change> Changes { get; set; }
    }

    /// <summary>
    /// Change used in a Patch
    /// </summary>
    [JsonObject(id: "change")]
    public class Change
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
