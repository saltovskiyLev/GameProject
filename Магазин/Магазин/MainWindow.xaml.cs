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
using System.Diagnostics;

namespace Магазин
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<CartElement> Cart = new List<CartElement>();
        int[] SelectetProduct = new int[2];
        int SelectetTab = 0;
        bool fullSize = false;
        Product[] AllFood;
        Product[] ComputerParts;
        string path = "";
        public MainWindow()
        {
            InitializeComponent();
            SelectetProduct[0] = -1;
            SelectetProduct[1] = -1;
            string[] goods;
            path = Environment.CurrentDirectory;
            int p = Environment.CurrentDirectory.LastIndexOf('\\');
            if (Environment.CurrentDirectory.Substring(p + 1) == "Debug")
            {
                path = "..\\..\\";
            }
            ComputerParts = ReadProducts(path, "Комплектующие для ПК.txt");
            PartsList.ItemsSource = ComputerParts;
            AllFood = ReadProducts(path, "Еда и доставка.txt");
            FoodList.ItemsSource = AllFood;
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
        private void PartsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product p = (Product)(PartsList.SelectedItem);
            ShowProduct(p);
            SelectetProduct[0] = PartsList.SelectedIndex;
        }
        void ShowProduct(Product item)
        {
            Price.Text = "Цена: " + item.Price.ToString();
            Number.Text = "Количество: " + item.Number.ToString();
            var uri = new Uri(Directory.GetCurrentDirectory() + "\\" + item.Picture);
            var img = new BitmapImage(uri);
            Picture.Source = img;
            BorderImage.Visibility = Visibility.Visible;
        }

        private void FoodList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product p = (Product)(FoodList.SelectedItem);
            ShowProduct(p);
            SelectetProduct[1] = FoodList.SelectedIndex;
        }

        private void Catalog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Debug.WriteLine(e.Source);
            if (e.Source is TabControl t)
            {
                SelectetTab = t.SelectedIndex;
                if(SelectetProduct[SelectetTab] == -1)
                {
                    Price.Text = "";
                    Number.Text = "";
                    Picture.Source = null;
                    BorderImage.Visibility = Visibility.Hidden;
                    return;
                }
                if (SelectetTab == 0)
                {
                    ShowProduct(ComputerParts[SelectetProduct[SelectetTab]]);
                }
                if(SelectetTab == 1)
                {
                    ShowProduct(AllFood[SelectetProduct[SelectetTab]]);
                }
            }
        }
        private void Picture_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!fullSize)
            {
                fullSize = true;
                Picture.Width = 720;
            }
            else
            {
                fullSize = false;
                Picture.Width = 320;
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            CartElement purchase = new CartElement();
            if (Catalog.SelectedIndex == 0)
            {
                if (PartsList.SelectedIndex != -1)
                {
                    purchase.Product = ComputerParts[PartsList.SelectedIndex];
                }
            }
            else if(Catalog.SelectedIndex == 1)
            {
                if (FoodList.SelectedIndex != -1)
                {
                    purchase.Product = AllFood[FoodList.SelectedIndex];
                }
            }
            if(purchase.Product == null)
            {                                                                                                       
                return;
            }
            purchase.Quantity = 1;
            bool IsFound = false;
            for (int i = 0; i < Cart.Count; i++)
            {
                if (purchase.Name == Cart[i].Name)
                {
                    IsFound = true;
                    Cart[i].Quantity ++;
                }
            }
            if (IsFound == false)
            {
                Cart.Add(purchase);
            }
            MessageBox.Show("Товар успешно добавлен в корзину.");
        }

        private void ShowCart_Click(object sender, RoutedEventArgs e)
        {
            CartWindow w = new CartWindow(Cart);
            w.ShowDialog();
        }
    }
    public class CartElement
    {
        public Product Product;
        public int Quantity { get; set; }
        public string Price
        {
            get
            {
                return Product.Price.ToString();
            }
        }
        public string Name
        {
            get
            {
                return Product.Name;
            }
        }
        public int Sum
        {
            get
            {
                return Quantity * Product.Price;
            }
        }
    }
}
