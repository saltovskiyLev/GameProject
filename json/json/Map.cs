using System.Collections.Generic;
using Newtonsoft.Json;

namespace json
{
    class Map
    {
        [JsonProperty("mapName")]
        public string MapName { get; set; }
        [JsonProperty("SizeX")]
        public int XCells { get; set; }
        [JsonProperty("SizeY")]
        public int YCells { get; set; }
        public List<GameObject> Objects { get; set; }

        public Map()
        {
            Objects = new List<GameObject>();
        }
    }
}
