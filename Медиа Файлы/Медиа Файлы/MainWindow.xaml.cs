using System;
using System.Collections.Generic;
using System.IO;
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

namespace Медиа_Файлы
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            video.Source = new Uri(@"C:\Users\Admin\Documents\GitHub\GameProject\Медиа Файлы\Медиа Файлы\windows-xp-critical-stop.mp3");
        }
        MediaPlayer Audio = new MediaPlayer();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string FileName = Directory.GetCurrentDirectory() + "\\Jingle_Lose_00.mp3";
            Audio.Open(new Uri(FileName));
            Audio.Play();
            var data = File.ReadAllBytes(FileName);
        }
    }
}