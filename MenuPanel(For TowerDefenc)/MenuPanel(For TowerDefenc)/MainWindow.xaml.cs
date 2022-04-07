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
            hello.Children.Add(Menu);
            Menu.AddTab("Танки");
            Menu.AddTab("Здания");
        }
        MenuPanel Menu = new MenuPanel();
    }

    public class MenuPanel : TabControl
    {
        Dictionary<string, MenuTab> Tabs = new Dictionary<string, MenuTab>();
        public void AddTab(string TabName)
        { 
            MenuTab Tab = new MenuTab(TabName);
            Items.Add(Tab);
            Tabs.Add(TabName, Tab);
        }
    }

    public class MenuTab : TabItem
    {
        public WrapPanel Panel = new WrapPanel();
        public MenuTab(string TabName)
        {
            Content = Panel;
            Header = new TextBlock { Text = TabName };
        }
    }
}
