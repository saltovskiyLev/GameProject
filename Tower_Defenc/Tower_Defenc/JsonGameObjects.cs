using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace Tower_Defenc
{
    public class JsonGameObjects
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("speed")]
        public double speed { get; set; }
        [JsonProperty("CanClash")]
        public bool CanClash { get; set; }
        [JsonProperty("ChargeSpeed")]
        public int ChargeSpeed { get; set; }
        [JsonProperty("ChargeReady")]
        public int ChargeReady { get; set; }
        [JsonProperty("Hp")]
        public int Hp { get; set; }
        [JsonProperty("MaxAmmo")]
        public int MaxAmmo { get; set; }
        [JsonProperty("Type")]
        public string Type { get; set; }
        [JsonProperty("PictureSet")]
        public List<string> PictureSet { get; set; }
        [JsonProperty("MaxSide")]
        public int MaxSide { get; set; }
        [JsonProperty("DestroyedImage")]
        public string DestroyedImage { get; set; }
        [JsonProperty("Range")]
        public int Range { get; set; }


        static int counter = 0;

        static public GameObject GetJsonGO(string path)
        {
            counter++;
            string str = File.ReadAllText(path);
            JsonGameObjects obj1 = JsonConvert.DeserializeObject<JsonGameObjects>(str);
            GameObject obj;
            if (obj1.PictureSet.Count == 1)
            {
                obj = new GameObject(obj1.PictureSet[0] , obj1.name + counter, obj1.Type);
            }
            else
            {
                obj = new GameObject(obj1.PictureSet[0], obj1.PictureSet[1], obj1.name + counter, obj1.Type);
            }
            obj.Speed = obj1.speed;
            obj.CanClash = obj1.CanClash;
            /*obj.Recharger = new SimpleRechargen();//
            obj.Recharger.ChargeSpeed = obj1.ChargeSpeed;
            obj.Recharger.ChargeReady = obj1.ChargeReady;*/
            obj.SetHp(obj.HP);
            obj.MaxAmmo = obj1.MaxAmmo;
            obj.AddAmmo(obj1.MaxAmmo);
            MainWindow.map.ContainerSetMaxSide(obj.ContainerName, obj1.MaxSide);
            obj.Range = obj1.Range;
            obj.destroyedImage = obj1.DestroyedImage;
            return obj;
        }
    }

}

