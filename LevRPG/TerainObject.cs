using Newtonsoft.Json;

namespace LevRPG
{
    public class TerainObject
    {
        [JsonProperty("x1")]
        public int x1;
        [JsonProperty("y1")]
        public int y1;

        [JsonProperty("x2")]
        public int x2;
        [JsonProperty("y2")]
        public int y2;

        [JsonProperty("type")]
        public string type;

        [JsonProperty("image")]
        public string Image;

        [JsonProperty("id")]
        public string Id;
    }
}