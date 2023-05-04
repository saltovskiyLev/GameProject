using Newtonsoft.Json;

namespace EditorForLevRpg
{
    public class TerainObject:TerrainObjectTemplate
    {
        [JsonProperty("x1")]
        public int x1;
        [JsonProperty("y1")]
        public int y1;

        [JsonProperty("x2")]
        public int x2;
        [JsonProperty("y2")]
        public int y2;

        [JsonProperty("id")]
        public string Id;

        public void RemoveObject()
        {
            MainWindow.RemoveObjects(Id);
        }
    }


}
