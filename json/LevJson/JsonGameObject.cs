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
        [JsonProperty("StringParams")]
        public JDictionary<string, string> StringParams { get; set; }
        [JsonProperty("IntParams")]
        public JDictionary<string, int> IntParams { get; set; }

        public JsonGameObject()
        {
            StringParams = new JDictionary<string, string>();
            IntParams = new JDictionary<string, int>();
    }

}
}