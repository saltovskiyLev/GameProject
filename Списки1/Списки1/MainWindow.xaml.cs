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

namespace Списки1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> Towns = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            Towns.Add("Moscow");
            Towns.Add("Dubna");
            DisplayList();
        }
        void DisplayList ()
        {
            Result.Text = "";
            for (int i = 0; i < Towns.Count; i++)
            {
                Result.Text += Towns[i] + "\n";
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Towns.Add(Data.Text);
            DisplayList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Towns.Remove(Data.Text);
            DisplayList();
        }
    }
}
