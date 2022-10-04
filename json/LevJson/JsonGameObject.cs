using System.Collections.Generic;
using Newtonsoft.Json;

namespace LevJson
{
    public class JsonGameObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
    }
}