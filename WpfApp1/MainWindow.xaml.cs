using System;
using System.IO;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer mediaPlayer = new MediaPlayer();
        MediaPlayer mediaPlayer2 = new MediaPlayer();
        string[] files;
        int fileNum = 0;

        public MainWindow()
        {
            InitializeComponent();
            files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\sounds");
            video.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Open(new Uri(files[fileNum]));
            mediaPlayer.Play();
            fileNum++;
            mediaPlayer2.Open(new Uri(files[fileNum]));
            mediaPlayer2.Play();
            if (fileNum < files.Length - 1) fileNum++;
            else fileNum = 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            video.Play();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            video.Stop();
        }
    }
}
