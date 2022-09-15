using System.Collections.Generic;
using Newtonsoft.Json;

namespace LevJson
{
    class JsonMap
    {
        [JsonProperty("mapName")]
        public string MapName { get; set; }
        [JsonProperty("SizeX")]
        public int XCells { get; set; }
        [JsonProperty("SizeY")]
        public int YCells { get; set; }
        public List<JsonGameObject> Objects { get; set; }

        public JsonMap()
        {
            Objects = new List<JsonGameObject>();
        }
    }
}
