using System;

namespace strings
{
    class Program
    {
        static int UsersNumber = 2;
        static string[] Paswords;
        static string[] Users;
        static int GoodsNumber;
        static int[] warehouse;
        static string[] Goods;
        static double[] price;
        static void Main(string[] args)
        {
            string command = "";
            GoodsNumber = 5;
            price = new double[GoodsNumber];
            price[0] = 12000;
            price[1] = 20000;
            price[2] = 10000;
            price[3] = 7000;
            price[4] = 8000;
            Goods = new string[GoodsNumber];
            Goods[0] = "процессор";
            Goods[1] = "видеокарта";
            Goods[2] = "материнская плата";
            Goods[3] = "оперативная память";
            Goods[4] = "корпус";
            warehouse = new int[GoodsNumber];
            warehouse[0] = 1;
            warehouse[1] = 2;
            warehouse[2] = 1;
            warehouse[3] = 1;
            warehouse[4] = 0;
            Users = new string[UsersNumber];
            Users[0] = "Leo";
            Users[1] = "Misha";
            Paswords = new string[UsersNumber];
            Paswords[0] = "LeoLev2020";
            Paswords[1] = "123123123";
            string UserName = "";
            Console.Write("введите имя: ");
            UserName = Console.ReadLine();
            for(int i = 0; i < UsersNumber; i++)
            {
                if (UserName == Users[i])
                {
                    Console.Write("введите пароль: ");
                    if (Console.ReadLine() == Paswords[i])
                    {
                        Console.WriteLine("Добро пожаловать!");
                    }
                    else
                    {
                        Console.WriteLine("Пароль неверный");
                        Console.ReadKey();
                        return;
                    }
                }
            }
            while (command !=  "выход")
            {
                Console.Write("введите команду: ");
                command = Console.ReadLine();
                if(command == "товары")
                {
                    ShowPriceList();
                }
                if (command == "цена")
                {
                    Console.Write("введите название товара: ");
                    ShowPrice(Console.ReadLine());
                }
            }
        }
        static void ShowPriceList()
        {
            for(int i = 0; i < Goods.Length; i++)
            {
                Console.WriteLine(Goods[i] + " : " + price[i] + ", осталось " + warehouse[i]);
            }   
         }
        static void ShowPrice(string item)
        {
            for(int i = 0; i < Goods.Length; i++)
            {
                if(item == Goods[i])
                {
                    Console.WriteLine(price[i]);
                    return;
                }
            }
            Console.WriteLine("товар не найден");
        }
    }
}
