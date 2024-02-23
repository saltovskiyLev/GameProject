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
using System.IO;

namespace LeoJourney
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Scene CurrentScene;
        TextBlock[] Variants = new TextBlock[4];
        string ProjectPath = "E:\\GameProject\\GameProject\\LeoJourney\\";
        string path; //@"C:\Users\Admin\Documents\GitHub\GameProject\LeoJourney\LeoJourney\Levels\";
        public MainWindow()
        {
            InitializeComponent();
            path = ProjectPath + "LeoJourney\\Levels\\";
            Variants[0] = tbVariant1;
            Variants[1] = tbVariant2;
            Variants[2] = tbVariant3;
            Variants[3] = tbVariant4; 
            PanelBackground.ImageSource = new BitmapImage(new Uri(ProjectPath + "Pushka.jpg"));
        }
        Scene ReadScene(string Id)
        {
            string SceneText = File.ReadAllText(path + "Сцена_" + Id + ".txt");
            Scene scene = new Scene();
            string[] sceneParams = SceneText.Split(new char[] {'#'}, StringSplitOptions.RemoveEmptyEntries);
            scene.Id = sceneParams[0];
            scene.Picture = sceneParams[1];
            scene.Description = sceneParams[2];
            scene.Variants = new Variant[4];
            for(int i = 3; i < sceneParams.Length; i++)
            {
                string[] Variants = sceneParams[i].Split(new char[] { '$' });
                scene.Variants[i - 3] = new Variant();
                scene.Variants[i - 3].Description = Variants[0];
                scene.Variants[i - 3].SceneId = Variants[1];
            }
            return scene;
        }

        private void Button_Click_NewGame(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            CurrentScene = ReadScene("1");
            ChangeScene("1");
            GamePanel.Visibility = Visibility.Visible;
        }

        private void tbVariant1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeScene(CurrentScene.Variants[0].SceneId);
        }

        private void tbVariant2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeScene(CurrentScene.Variants[1].SceneId);
        }

        private void tbVariant3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeScene(CurrentScene.Variants[2].SceneId);
        }

        private void tbVariant4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeScene(CurrentScene.Variants[3].SceneId);
        }
        void ChangeScene(string sceneId)
        {
            Scene scene = ReadScene(sceneId);
            DisplayScene(scene);
            CurrentScene = scene;
            BitmapImage img = new BitmapImage(new Uri(ProjectPath + scene.Picture));
            Picture.Source = img;
        }
        private void DisplayScene(Scene scene)
        {
            tbScene.Text = scene.Description;
            for (int i = 0; i < scene.Variants.Length; i++)
            {
                if(scene.Variants[i] != null)
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
     
    }
    public class Variant
    {
        public string SceneId;
        public string Description;
    }
    public class Scene
    {
        public string Id;         
        public string Picture;    
        public string Description;
        public Variant[] Variants; 
    }
}