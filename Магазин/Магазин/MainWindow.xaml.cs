using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

namespace Магазин
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Product[] AllFood;
        Product[] ComputerParts;
        string path = "";
        public MainWindow()
        {
            InitializeComponent();
            string[] goods;
            path = Environment.CurrentDirectory;
            int p = Environment.CurrentDirectory.LastIndexOf('\\');
            if (Environment.CurrentDirectory.Substring(p + 1) == "Debug")
            {
                path = "..\\..\\";
            }
            ComputerParts = ReadProducts(path, "Комплектующие для ПК.txt");
            PartsList.ItemsSource = ComputerParts;
            //Product p1 = new Product();
            //p1.SetFields(goods[0]);
        }
        string GetUserFileName()
        {
            return path + "Пользователи\\" + UserName.Text + ".TXT";
        }
        Product[] ReadProducts(string path, string fileName)
        {
            string[] goods;
            Product[] items;
            goods = File.ReadAllLines(path + "Данные о товаре\\" + fileName);
            items = new Product[goods.Length];
            for (int i = 0; i < goods.Length; i++)
            {
                items[i] = new Product();
                items[i].SetFields(goods[i]);
            }
            return items;
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(GetUserFileName()))
            {
                string[] data = File.ReadAllLines(GetUserFileName());
                if (Password.Text == data[0])
                {
                    LoginPanel.Visibility = Visibility.Collapsed;
                    Shop.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Неправильное имя или пароль");
                }
            }
            else
            {
                MessageBox.Show("Неправильное имя или пароль");
            }    
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(GetUserFileName()))
            {
                MessageBox.Show("Такое имя уже используется");
            }
            else
            {
                File.WriteAllText(GetUserFileName(), Password.Text);
                MessageBox.Show("Регистрация прошла успешно");
                LoginPanel.Visibility = Visibility.Collapsed;
                Shop.Visibility = Visibility.Visible;
            }
        }
    }
     public class Product
    {
        public int Price { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public bool IsAvailible
        {
            get
            {
                if (Number > 0)
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
        public void SetFields(string item)
        {
            string[] Fields = item.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            Name = Fields[0];
            Price = int.Parse(Fields[1]);
            Number = int.Parse(Fields[2]);
            Picture = Fields[3];
        }
    }
}
