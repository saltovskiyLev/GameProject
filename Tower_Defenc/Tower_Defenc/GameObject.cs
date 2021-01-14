using GameMaps;

namespace Tower_Defenc
{
    class GameObject
    {
        static public UniversalMap_Wpf map;
        public int X;
        public int Y;
        public string ContainerName;
        /// <summary>
        /// Пошли функции
        /// </summary>
        public GameObject(string PICTURE, string Container)
        {
            map.Library.AddContainer(Container, PICTURE);
            ContainerName = Container;
        }
        public GameObject(string PICTURE, string Container, int x, int y, int size): this(PICTURE, Container)
        {
            map.ContainerSetMaxSide(ContainerName, 70);
            SetCoordinate(x, y);
        }
        public void SetCoordinate(int x, int y)
        {
            X = x;
            Y = y;
            map.ContainerSetCoordinate(ContainerName, x, y);
        }
    }
}