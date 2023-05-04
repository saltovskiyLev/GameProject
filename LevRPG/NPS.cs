using Newtonsoft.Json;

namespace LevRPG
{
    class NPC
    {
        [JsonProperty ("CenterX")]
        public double CenterX;

        [JsonProperty("CenterY")]
        public double CenterY;

        [JsonProperty("Name")]
        public string Name;

        [JsonProperty("Id")]
        public string id;

        [JsonProperty("Image")]
        public string Image;

        [JsonProperty("Angle")]
        public int Angle;

        [JsonProperty("MaxSide")]
        public int MaxSide;

        [JsonProperty("Speed")]
        public int Speed;

        [JsonProperty("hp")]
        public int hp;


        static public void npc()
        {

        }
    }

}
