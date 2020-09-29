﻿using System;
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
        string path = @"C:\Users\Admin\Documents\GitHub\GameProject\LeoJourney\LeoJourney\Levels\";
        public MainWindow()
        {
            InitializeComponent();
            PanelBackground.ImageSource = new BitmapImage(new Uri("C:\\Users\\Admin\\Documents\\GitHub\\GameProject\\LeoJourney\\Pushka.jpg"));
        }
        Scene ReadScene(string Id)
        {
            string SceneText = File.ReadAllText(path + "Сцена_" + Id + ".txt");
            Scene scene = new Scene();
            string[] sceneParams = SceneText.Split(new char[] {'#'});
            scene.Id = sceneParams[0];
            scene.Picture = sceneParams[1];
            scene.Description = sceneParams[2];
            scene.Variants = new Variant[4];
            for(int i = 3; i < sceneParams.Length; i++)
            {
                string[] Variants = SceneText.Split(new char[] { '$' });
                scene.Variants[i - 3] = new Variant();
                //scene.Variants[i - 3].Description = Variants[];
            }
            return scene;
        }

        private void Button_Click_NewGame(object sender, RoutedEventArgs e)
        {
            Scene scene;
            MainMenu.Visibility = Visibility.Collapsed;
            scene = ReadScene("1");
            DisplayScene(scene);
            GamePanel.Visibility = Visibility.Visible;
        }

        private void tbVariant1_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void tbVariant2_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void tbVariant3_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void tbVariant4_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        private void DisplayScene(Scene scene)
        {
            tbScene.Text = scene.Description;
        }

    }
    public class Variant
    {
        public int SceneId;
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
