using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Квест_2022
{
    /// <summary>
    /// Interaction logic for ShopWindow.xaml
    /// </summary>
    public partial class ShopWindow : Window
    {
        public Dictionary<string, int> ShopItems;
        public Dictionary<string, int> PlayerItems;
        public ShopWindow()
        {
            InitializeComponent();
            
        }
        public void Init()
        {
            TBmoney.Text = PlayerItems["coin"].ToString();
            foreach (string key in PlayerItems.Keys)
            {
                ShopItemUI item = new ShopItemUI(key, PlayerItems[key]);
                WPplayerItems.Children.Add(item);

            }
        }
    }
}
