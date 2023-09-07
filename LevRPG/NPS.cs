using Newtonsoft.Json;
using GameMaps;
using System;

namespace LevRPG
{
    class NPC : ICoordinateProvider
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

        [JsonIgnore]
        public ICoordinateProvider Target;

        [JsonIgnore]
        public string ContainerName;

        public double GetX()
        {
            return CenterX;
        }

        public double GetY()
        {
            return CenterY;
        }

        public void Move()
        {


            if (Target == null) return;

            double DistanceX = Target.GetX() - CenterX;
            double DistanceY = Target.GetY() - CenterY;

            if (Math.Abs(DistanceX) < 3 && Math.Abs(DistanceY) < 3)
            {
                return;
            }


                double AngleToTarget = GameMath.GetAngleOfVector(DistanceX, DistanceY);


            double dX = Speed * Math.Cos(GameMath.DegreesToRad(AngleToTarget));
            double dY = Speed * Math.Sin(GameMath.DegreesToRad(AngleToTarget));




            CenterX = CenterX + dX;
            CenterY = CenterY + dY;

            MainWindow.map.ContainerSetCoordinate(ContainerName, CenterX, CenterY);
        }
    }

}
