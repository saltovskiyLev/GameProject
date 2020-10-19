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
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Редактор_Урованей
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBox[] Variants = new TextBox[4];
        List<Scene> scenes = new List<Scene>();
        string path = "C:\\Users\\Admin\\Documents\\GitHub\\GameProject\\LeoJourney\\LeoJourney\\Levels";
        public MainWindow()
        {
            InitializeComponent();
            Variants[0] = tbVariant_1;
            Variants[1] = tbVariant_2;
            Variants[2] = tbVariant_3;
            Variants[3] = tbVariant_4;
            string[] Files;
            Files = Directory.GetFiles(path);
            for(int i = 0; i < Files.Length; i++)
            {
                Scene scene = Scene.ReadScene(Files[i]);
                scenes.Add(scene);
            }
            lbScenes.ItemsSource = scenes;
        }
        private void DisplayScene(Scene scene)
        {
            tbSceneText.Text = scene.Description;
            tbScenePicture.Text = scene.Picture;
            for (int i = 0; i < scene.Variants.Length; i++)
            {
                if (scene.Variants[i] != null)
                {
                    Variants[i].Text = scene.Variants[i].Description;
                    Variants[i].Visibility = Visibility.Visible;
                }
                else
                {
                    Variants[i].Visibility = Visibility.Collapsed;
                }
            }
        }

        private void lbScenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayScene(lbScenes.SelectedItem as Scene);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Scene s = (lbScenes.SelectedItem as Scene);
            string sceneStr = s.GetString();
        }
    }
    public class Variant
    {
        public string SceneId;
        public string Description;
    }
    public class Scene
    {
        public string Id { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public Variant[] Variants { get; set; } 
        static public Scene ReadScene(string path)
        {
            string SceneText = File.ReadAllText(path);
            Scene scene = new Scene();
            string[] sceneParams = SceneText.Split(new char[] { '#' });
            scene.Id = sceneParams[0];
            scene.Picture = sceneParams[1];
            scene.Description = sceneParams[2];
            scene.Variants = new Variant[4];
            for (int i = 3; i < sceneParams.Length; i++)
            {
                string[] Variants = sceneParams[i].Split(new char[] { '$' });
                scene.Variants[i - 3] = new Variant();
                scene.Variants[i - 3].Description = Variants[0];
                scene.Variants[i - 3].SceneId = Variants[1];
            }
            return scene;
        }
        public string GetString()
        {
            string str = "";
            str = str + Id;
            str += "#";
            str += Picture;
            str += "#";
            str += Description;
            str += "#";
            for(int i = 0; i < Variants.Length; i++)
            {
                if (Variants[i] != null)
                {
                    str += Variants[i].Description;
                    str += "$";
                    str += Variants[i].SceneId;
                }
            }
            return str;
        }
    }
}