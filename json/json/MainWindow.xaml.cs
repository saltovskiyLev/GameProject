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
using Newtonsoft.Json;
using System.IO;

namespace json
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Map m = new Map();
            m.MapName = "map01";
            m.XCells = 10;
            m.YCells = 26;

            GameObject player = new GameObject();
            player.Name = "player";
            player.X = 12;
            player.Y = 14;

            m.Objects.Add(player);

            player = new GameObject();
            player.Name = "enemy";
            player.X = 32;
            player.Y = 16;

            m.Objects.Add(player);

            SerializeMap(m);
            Deserialize("TestMap01.json");
        }

        void SerializeMap(Map map)
        {
            string s = JsonConvert.SerializeObject(map);
            File.WriteAllText("TestMap.json", s);
        }

        void Deserialize(string path)
        {
            string s = File.ReadAllText(path);
            Map m = JsonConvert.DeserializeObject<Map>(s);
        }
    }
}
