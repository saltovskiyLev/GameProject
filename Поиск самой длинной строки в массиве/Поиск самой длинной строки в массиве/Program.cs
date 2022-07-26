using System;

namespace Поиск_самой_длинной_строки_в_массиве
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] str = new string[5];
            str[0] = "Город";
            str[1] = "А";
            str[2] = "БfsjuijgudgБ";
            str[3] = "ВВfffffffВ";
            str[4] = "ГГГГ";

            int index = 0;
            int MaxLength = str[index].Length;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Length > MaxLength)
                {
                    MaxLength = str[i].Length;
                    index = i;
                }
            }
            Console.WriteLine(str[index]);
            Console.ReadKey();  
        }
    }
}
