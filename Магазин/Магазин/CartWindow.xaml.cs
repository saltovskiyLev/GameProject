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

namespace Магазин
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        public List<CartElement> UserCart;
        public CartWindow(List <CartElement> cart)
        {
            InitializeComponent();
            UserCart = cart;
            lbCart.ItemsSource = UserCart;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Функция вызвана -");
            CartElement Item = lbCart.SelectedItem as CartElement;
            MessageBox.Show(Item.Name);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Функция вызвана +");
        }
    }
}
