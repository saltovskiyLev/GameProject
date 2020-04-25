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

namespace тест
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        int Clicks = 0;
        public MainWindow()
        {
            InitializeComponent();
            Hello.Text = Clicks.ToString();
            Result.Text = "Здесь будет ваш результат";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hello.FontSize += 2;
            Clicks++;
            Hello.Text = Clicks.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
