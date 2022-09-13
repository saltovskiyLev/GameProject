﻿
using Newtonsoft.Json;

namespace json
{
    class GameObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
    }
}
