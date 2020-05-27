using System;
using System.Collections.Generic;
using System.IO;
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

namespace Классы
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Product p2;
        Product p;
        public MainWindow()
        {
            InitializeComponent();
            File.ReadAllLines("..\\..\\Данные о товаре\\Товары.txt");
            p = new Product();
            p.Name = "процессор СORE i5";
            p.Price = 15000;
            p.Number = 120;
            p2 = new Product();
            p2.Name = "видеокарта MSI GeForce RTX 2080 Ti 1635MHz";
            p2.Price = 40000;
            p2.Number = 2;
            /*Coordinate c;                                                                                                                                                                                 
            Coordinate c1;
            c = new Coordinate();
            c.x = 5;
            c.y = 12;
            c1 = c;
            c1.x = 100;
            TB.Text = c1.x.ToString() + "," + c1.y.ToString();*/
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            TB.Text = p2.GetInfo();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            TB.Text = p.GetInfo();
        }
    }
    /*class Coordinate
    {
        public int x;
        public int y;
    }*/
    class Product
    {
        public int Price;
        public int Number;
        public string Name;
        public string Picture;
        public bool IsAvailible
        {
            get
            {
                if(Number > 0)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
        }
        public string GetInfo()
        {
            string Info = "";
            Info = "название: " + Name + "\n";
            Info += "цена: " + Price.ToString() + "\n";
            Info += "количество на складе: " + Number.ToString();
            return Info;
        }
    }
    /*class Product2
    {
        public int Price;
        public int Number;
        public string Name;
        public string Picture;
        public bool IsAvailible
        {
            get
            {
                if(Number > 0)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
        }
        public string GetInfo()
        {
            string Info = "";
            Info = "название: " + Name + "\n";
            Info += "цена: " + Price.ToString() + "\n";
            Info += "количество на складе: " + Number.ToString();
            return Info;
        }
    }*/
}
