using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace EditorForLevRpg
{
    public class MapData
    {
        [JsonProperty("terainObjects")]
        public List<TerainObject> terainObjects = new List<TerainObject>();

        static public MapData GetMapFromeFile(string path)
        {
            MapData data;
            string str = File.ReadAllText(path);
            data = JsonConvert.DeserializeObject<MapData>(str);




            return data;
        }
    }

}
