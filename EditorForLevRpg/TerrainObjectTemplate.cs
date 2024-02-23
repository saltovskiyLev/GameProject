using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EditorForLevRpg
{
    public class TerrainObjectTemplate
    {
        [JsonProperty("type")]
        public string type;

        [JsonProperty("image")]
        public string Image;

        static public List<TerrainObjectTemplate> ReadObjects(string path)
        {
            List<TerrainObjectTemplate> terrainObjectTemplates = new List<TerrainObjectTemplate>();

            string[] str = File.ReadAllLines(path);

            for(int i = 0; i < str.Length; i++)
            {
                string[] objdata = str[i].Split(',');

                terrainObjectTemplates.Add(new TerrainObjectTemplate
                {
                    type = objdata[0],
                    Image = objdata[1]
                });
            }

            return terrainObjectTemplates;
        }
    }
}
