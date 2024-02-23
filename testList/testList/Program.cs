using System;
using System.Collections.Generic;

namespace testList
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Box> boxes = new List<Box>();

            Box b = new Box();
            b.IsOnPlace = true;
            b.X = 12;
            b.Y = 1;

            boxes.Add(b);

            Box c = new Box();
            c.IsOnPlace = true;
            c.X = 123;
            c.Y = 11;

            boxes.Add(c);

            boxes[1].X = 1241;

            b.Y = 100;

        }

        class Box
        {
            public int X;
            public int Y;
            public bool IsOnPlace;
        }
    }
}
