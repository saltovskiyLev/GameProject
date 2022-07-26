using GameMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Квест_2022
{
    class GameObject
    {
        static public UniversalMap_Wpf map;
        public int X { get; private set; }
        public int Y { get; private set; }
        string PictureName;
        Dictionary<string, int> Params = new Dictionary<string, int>();
        Dictionary<string, int> Items = new Dictionary<string, int>();

        public GameObject(string pictureName, int x, int y)
        {
            X = x;
            Y = y;
            PictureName = pictureName;
            MoveTo(x, y);
        }

        public void MoveTo(int x, int y)
        {
            map.RemoveFromCell(PictureName, X, Y);
            X = x;
            Y = y;
            map.DrawInCell(PictureName, x, y);
        }
    }
}
