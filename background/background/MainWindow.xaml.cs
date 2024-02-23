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

namespace background
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Uri res = new Uri(@"C:\Users\Admin\Documents\GitHub\GameProject\редактор карт\GameProject\images\evil1.png");
            BitmapImage img = new BitmapImage(res);
            ImageBrush br = new ImageBrush(img);
            canvas.Background = br;
        }
    }
}
