using System;
using System.Collections.Generic;

namespace Interface
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IShoot> ZOMBYS = new List<IShoot>();
            GameLeva leva = new GameLeva();
            GameErnest ernest = new GameErnest();
            ZOMBYS.Add(ernest);
            ZOMBYS.Add(leva);
            for(int i = 0; i < ZOMBYS.Count; i++)
            {
                ZOMBYS[i].shoot();
            }
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
    interface IShoot
    {
        void shoot();
    }
    class GameLeva : IShoot
    {
        public void shoot()
        {
            Console.WriteLine("leva : БАААААБАААААААААAAAAAAAAAAAAAAAAAAAAAAAAААХ");
            //throw new NotImplementedException();
        }
    }
    class GameErnest : IShoot
    {
        public void shoot()
        {
            Console.WriteLine("Ernest : БААААААААААААAAAAAAAAAAAAAAAAAААААААБАХ");
        }
    }
}
