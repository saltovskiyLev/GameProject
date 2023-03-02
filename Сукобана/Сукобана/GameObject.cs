using GameMaps;

namespace Сукобана
{
    public class GameObject
    {
        public int X;
        public int Y;
        public string PictureName;

        public GameObject(string pictureName, int x, int y)
        {
            X = x;
            Y = y;
            PictureName = pictureName;
            Move(x, y);
        }

        public void Move(int x, int y)
        {
            MainWindow.map.RemoveFromCell(PictureName, X, Y);
            X = x;
            Y = y;
            MainWindow.map.DrawInCell(PictureName, x, y);
        }
    }
}
