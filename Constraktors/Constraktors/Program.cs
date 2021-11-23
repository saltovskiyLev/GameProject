using System;

namespace Constraktors
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            GameObject player = new GameObject(9, 7);
        }
    }
    class GameObject
    {
        int x, y;
        public void SetCoordinate(int NewX, int NewY)
        {
            x = NewX;
            y = NewY;
        }
        public GameObject(int StartX, int StartY)
        {
            x = StartX;
            y = StartY;
        }

    }
}
