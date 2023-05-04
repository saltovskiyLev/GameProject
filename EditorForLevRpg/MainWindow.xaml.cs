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
using GameMaps;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;

namespace EditorForLevRpg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TerrainObjectTemplate> terrainObjectTemplates;

        CellMapInfo mapInfo = new CellMapInfo(25, 25, 40, 0);
        static public UniversalMap_Wpf map;

        StackPanel SelectedUnit = null;

        string currentImage = "";

        bool IsKey = false;

        int x1;
        int y1;

        int Id = 0;

        static List<TerainObject> TerainObjects = new List<TerainObject>();


        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, mapInfo);
            SPmap.Children.Add(map.Canvas);

            terrainObjectTemplates = TerrainObjectTemplate.ReadObjects(@"..\\..\\objects\objects.txt");

            map.Mouse.SetMouseSingleLeftClickHandler(MapClick);

            AddPictures();
            CreateObjects();
        }


        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("rip2", "rip2.png");
            map.Library.AddPicture("wall1", "wall1.png");
            map.Library.AddPicture("wall2", "wall2.png");
        }

        public static void RemoveObjects(string Id)
        {
            map.ContainerErase(Id);

            for(int i = 0; i < TerainObjects.Count; i++)
            {
                if(TerainObjects[i].Id == Id)
                {
                    TerainObjects.RemoveAt(i);
                }
            }
        }

        void MapClick(int x, int y, int cellX, int cellY)
        {
            if(IsKey)
            {
                IsKey = false;
                TerainObject t = new TerainObject();
                t.x1 = x1;
                t.y1 = y1;
                t.x2 = x;
                t.y2 = y;
                t.Image = currentImage;
                t.Id = Id.ToString();
                Id++;

                TerainObjects.Add(t);

                DrawObj(t);
            }
            else
            {
                x1 = x;
                y1 = y;
                IsKey = true;
            }
        }

        void CreateObjects()
        {
            for (int i = 0; i < terrainObjectTemplates.Count; i++)
            {
                StackPanel SP = new StackPanel();
                WPObjects.Children.Add(SP);
                SP.Orientation = Orientation.Vertical;
                SP.Margin = new Thickness(10);
                string path = @"C:\Users\Admin\Documents\GitHub\GameProject\EditorForLevRpg\images\" + terrainObjectTemplates[i].Image + ".png";
                Uri uri = new Uri(path);
                BitmapImage bmp = new BitmapImage(uri);
                Image image = new Image();
                image.Source = bmp;
                image.Width = 40;
                image.Height = 40;

                SP.Children.Add(image);
                SP.Children.Add(new TextBlock
                {
                    Text = terrainObjectTemplates[i].type,
                    TextAlignment = TextAlignment.Center,
                    Tag = terrainObjectTemplates[i].Image
                });

                SP.MouseDown += Object_Click;
            }
        }

        private void WPObjects_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Object_Click(object sender, MouseButtonEventArgs e)
        {
            if (SelectedUnit != null)
            {
                SelectedUnit.Background = Brushes.White;
            }

            StackPanel sp = sender as StackPanel;
            sp.Background = Brushes.Red;

            SelectedUnit = sp;

            currentImage = (sp.Children[1] as TextBlock).Tag.ToString();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = ".json";
            saveFile.Filter = "JsonMap (.json)|*.json";
            saveFile.ShowDialog();
            MapData mAp = new MapData();
            mAp.terainObjects = TerainObjects;

            File.WriteAllText(saveFile.FileName, JsonConvert.SerializeObject(mAp));

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "JsonMap (.json)|*.json";
            openFile.ShowDialog();

            MapData mAp;
            mAp = JsonConvert.DeserializeObject<MapData>(File.ReadAllText(openFile.FileName));
            for(int i = 0; i < TerainObjects.Count; i++)
            {
                map.ContainerErase(TerainObjects[i].Id);
            }

            TerainObjects = mAp.terainObjects;

            for(int i = 0; i < TerainObjects.Count; i++)
            {
                DrawObj(TerainObjects[i]);
                
            }
        }

        void DrawObj(TerainObject t)
        {   
            map.Library.AddContainer(t.Id, t.Image, ContainerType.TiledImage);
            map.ContainerSetSize(t.Id, Math.Abs(t.x1 - t.x2), Math.Abs(t.y1 - t.y2));
            map.ContainerSetTileSize(t.Id, 40, 40);
            map.ContainerSetCoordinate(t.Id, (t.x1 + t.x2) / 2, (t.y1 + t.y2) / 2);
            map.ContainerSetLeftClickHandler(t.Id, ClickType.Right, t.RemoveObject);
        }
    }

}
