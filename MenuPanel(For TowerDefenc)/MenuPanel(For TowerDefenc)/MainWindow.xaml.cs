using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MenuPanel_For_TowerDefenc_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            grid.Children.Add(Menu);
            Test.MouseDown += Test_MouseDown_2;
            Menu.AddTab("Танки");
            Menu.AddTab("Здания");
            Menu.SetImageSize("Здания", 70);
            Menu.CreateNewItem("Тест", @"C:\Users\Admin\Documents\GitHub\GameProject\Tower_Defenc\Tower_Defenc\images\Танк с башней без поворота(ДОБРЫЙ СЛАБЫЙПУЛЕМЁТНЫЙ).png",
                "Тест#3", "Танки", TestLeftClickHandler);
            Menu.CreateNewItem("Тест#2", @"C:\Users\Admin\Documents\GitHub\GameProject\Tower_Defenc\Tower_Defenc\images\Танк с башней без поворота(ДОБРЫЙ средний).png",
                "Тест#2", "Танки", TestLeftClickHandler);
            Menu.CreateNewItem("Тест#3", @"C:\Users\Admin\Documents\GitHub\GameProject\Tower_Defenc\Tower_Defenc\images\i.png",
                "Тест#3", "Здания", TestLeftClickHandler);
        }

        void TestLeftClickHandler(string text)
        {
            MessageBox.Show(text);
        }

        MenuPanel Menu = new MenuPanel();

        private void Test_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Текст");
        }

        private void Test_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("HELLO");
        }

        private void button_1_Click(object sender, RoutedEventArgs e)
        {
            Test.MouseDown -= Test_MouseDown_2;
        }
    }
}