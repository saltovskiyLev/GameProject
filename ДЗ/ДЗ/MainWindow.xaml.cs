using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ДЗ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /*Программа хранит данные по объявлениям о сдаче квартир в аренду и выводит на экран перечень объявлений.
Задача: исправить программу, чтобы выводились верные адреса, текст выводился с переходами на следующую строку - проще говоря, чтобы было удобно читать.
Потом добавить четвертое объявление, исправить возникшую при этом ошибку в работе с массивом и обеспечить вывод на экран.
Для обхода проблемы с консольным приложением: создать приложение WPF, разместить внутрь Grid элемент TextBlock, дать ему имя(например, Flats - квартиры) и вместо Console.Write и Console.WriteLine использовать добавление текста к нему:
Flats.Text += "новый текст";
Для перехода на новую строку использовать служебное обозначение \n(внутри текста в кавычках).*/

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string[] Adresses = new string[4];
            int[] Rooms = new int[4];
            int[] Price = new int[4];

            Adresses[0] = "ул. Ленина, 25";
            Adresses[1] = "ул. Строителей, 3";
            Adresses[2] = "Никитский бульвар, 16";
            Adresses[3] = "ул. Московская, 12";

            Rooms[0] = 3;
            Rooms[1] = 1;
            Rooms[2] = 2;
            Rooms[3] = 4; 

            Price[0] = 55000;
            Price[1] = 30000;
            Price[2] = 75000;
            Price[3] = 45000;

            for (int i = 0; i < 4; i++)
            {
                Flats.Text += "Объявление №" + i.ToString() + "\n" + "Адрес: " + Adresses[i] + "\n" + "Кол-во комнат: " + Rooms[i].ToString() + "\n" + "Стоимость аренды (мес):" + Rooms[i].ToString() + "\n";
            }
        }
    }
}