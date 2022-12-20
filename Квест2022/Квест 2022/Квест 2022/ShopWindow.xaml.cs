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
using System.Windows.Shapes;
using System.Threading;
using System.Media;

namespace Квест_2022
{
    /// <summary>
    /// Interaction logic for ShopWindow.xaml
    /// </summary>
    public partial class ShopWindow : Window
    {
        public Dictionary<string, int> ShopItems;
        public Dictionary<string, int> PlayerItems;
        Timer timer;
        Action ShowText;
        public ShopWindow(BitmapImage avatar)
        {
            InitializeComponent();
            //imgAvatar.Source = avatar;
            avatarImage.Background = new ImageBrush(avatar);
            ShowText += SetOpacity;
            TimerCallback tb = new TimerCallback(CallBack);
            timer = new Timer(tb, null, 0, 20);
        }

        void CallBack(object obj = null)
        {
            if(Application.Current != null) Application.Current.Dispatcher.Invoke(ShowText);
        }

        public void Init()
        {
            // Заполнение панелей предметов игрока
            TBmoney.Text = PlayerItems["coin"].ToString();
            foreach (string key in PlayerItems.Keys)
            {
                if (key == "coin") continue;
                ShopItemUI item = new ShopItemUI(key, PlayerItems[key]);
                WPplayerItems.Children.Add(item);
            }
            // Заполнение предметов на продажу
           foreach(string key in ShopItems.Keys)
           {
                ShopItemUI item = new ShopItemUI(key, ShopItems[key]);
                ProductList.Children.Add(item);
                item.ItemName.MouseDown += Product_MouseDown;
           }
        }

        private void TBmoney_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (sender as TextBlock).FontSize = 30;
        }

        private void Product_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string productName = (sender as TextBlock).Text;
            if (ShopItems[productName] <= PlayerItems["coin"])
            {
                MessageBoxResult res;
                res = MessageBox.Show("Вы действительно хотите купить" + productName, "Покупка", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    PlayerItems["coin"] = PlayerItems["coin"] - ShopItems[productName];
                    if (PlayerItems.ContainsKey(productName))
                    {
                        PlayerItems[productName]++;
                        for(int i = 0; i < WPplayerItems.Children.Count; i++)
                        {
                            var s = WPplayerItems.Children[i] as ShopItemUI;
                            if (s.ItemName.Text == productName)
                            {
                                s.ItemPrice.Text = PlayerItems[productName].ToString();
                            }
                        }
                    }
                    else
                    {
                        PlayerItems.Add(productName, 1);
                        ShopItemUI item = new ShopItemUI(productName, 1);
                        WPplayerItems.Children.Add(item);
                    }
                    TBmoney.Text = PlayerItems["coin"].ToString();
                }
            }

            else
            {
                tbMessage.Text = "У вас недостаточно средств";
                tbMessage.Opacity = 1;
            }
        }

        void SetOpacity()
        {
            if(tbMessage.Opacity > 0)
            {
                tbMessage.Opacity -= 0.01;
            }
        }
    }
}
