using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace LeoJourney
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PanelBackground.ImageSource = new BitmapImage(new Uri("C:\\Users\\Admin\\Documents\\GitHub\\GameProject\\LeoJourney\\Pushka.jpg"));
        }
    }
    public class Variant
    {
        public int SceneId;
        public string Description;
    }
    public class Scene
    {
        public int Id;
        public string Picture;
        public string Description;
        public Variant[] Variants;
    }
}
