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
        bool editMode = true;
        TextBox[] Variants = new TextBox[4];
        TextBox[] VariantsId = new TextBox[4];
        List<Scene> scenes = new List<Scene>();
        string path = "C:\\Users\\Admin\\Documents\\GitHub\\GameProject\\LeoJourney\\LeoJourney\\Levels";
        public MainWindow()
        {
            InitializeComponent();
            Variants[0] = tbVariant_1;
            Variants[1] = tbVariant_2;
            Variants[2] = tbVariant_3;
            Variants[3] = tbVariant_4;
            //Это заполнение массивов
            VariantsId[0] = tbVariantId_1;
            VariantsId[1] = tbVariantId_2;
            VariantsId[2] = tbVariantId_3;
            VariantsId[3] = tbVariantId_4;
            ReadScene();
        }
        void ReadScene()
        {
            string[] Files;
            Files = Directory.GetFiles(path);
            for (int i = 0; i < Files.Length; i++)
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
                    // Variants[i].Visibility = Visibility.Visible;     
                    VariantsId[i].Text = scene.Variants[i].SceneId;   
                }
                else
                {
                    // Variants[i].Visibility = Visibility.Collapsed;
                    Variants[i].Text = "";
                    VariantsId[i].Text = "";
                }
            }
        }
        private void lbScenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayScene(lbScenes.SelectedItem as Scene);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Scene s;
            if(editMode == true)
            {
                s = (lbScenes.SelectedItem as Scene);
            }
            else
            {
                s = new Scene();
                s.Variants = new Variant[4];
                int Max = 0;
                for(int i = 0; i < scenes.Count; i++)
                {
                    if (int.Parse(scenes[i].Id) > Max)
                    {
                        Max = int.Parse(scenes[i].Id); 
                    }
                }
                s.Id = (Max + 1).ToString();
            }
            /*s.Variants[0].Description = tbVariant_1.Text;
            s.Variants[1].Description = tbVariant_2.Text;
            s.Variants[2].Description = tbVariant_3.Text;
            s.Variants[3].Description = tbVariant_4.Text;*/
            for (int i = 0; i < Variants.Length; i++)
            {
                if(Variants[i].Text != "")
                {
                    s.Variants[i] = new Variant();
                    s.Variants[i].Description = Variants[i].Text;
                    s.Variants[i].SceneId = VariantsId[i].Text;
                    // Я всё тут...
                    // Записать номер сцены к которой надо перейти.
                }
            }
            s.Description = tbSceneText.Text;
            s.Picture = tbScenePicture.Text;
            string sceneStr = s.GetString();
            Save_ToFile(s.Id, sceneStr);
            if (!editMode)
            {
                editMode = true;
                Clear();
                ReadScene();
                lbScenes.IsEnabled = true;
            }
        }
        private void Save_ToFile(string Id, string str)
        {
            string FileName = GetFileName(Id);
            File.WriteAllText(FileName, str);
        }
        string GetFileName(string ID)
        { 
            return path + "\\Сцена_" + ID + ".txt";
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if(editMode)
            {
                editMode = false;
                lbScenes.IsEnabled = false;
                Clear();
            }
        }
        private void Clear()
        {
            tbSceneText.Text = "";
            tbScenePicture.Text = "";
            for(int i = 0; i < 4; i++)
            {
                Variants[i].Text = ""; 
                VariantsId[i].Text = "";
            }
            /*for (int i = 0; i < VariantsId.Length; i++)
            {
                VariantsId[i].Text = "";
            }*/
        }
        private int GetMaxId()
        {
            int Max = 0;
            for (int i = 0; i < scenes.Count; i++)
            {
                if (int.Parse(scenes[i].Id) > Max)
                {
                    Max = int.Parse(scenes[i].Id);
                }
            }
            return Max;
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
            string[] sceneParams = SceneText.Split(new char[] { '#' },StringSplitOptions.RemoveEmptyEntries);
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
                    str += "#";
                }
            }
            return str;
        }
    }
}