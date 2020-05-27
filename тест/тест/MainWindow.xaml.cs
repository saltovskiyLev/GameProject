using System;
using System.IO;
using GameMaps;
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

namespace тест
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    ///
    ///
    ///
    ///
    ///
    /// </summary>
    public partial class MainWindow : Window
    {
        int BestResult = 0; 
        bool firstCall = true;
        TimerController Timer = new TimerController();
        bool Game = false;
        int Clicks = 0;
        public MainWindow()
        {
            InitializeComponent();
            Hello.Text = Clicks.ToString();
            string Resaut = File.ReadAllText("..\\..\\Результаты\\Result.TXT");
            Result.Text = "Лучший результат: " + Resaut;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (! Game)
            {
                Timer.AddAction(GameOver, 10000);
                Game = true;
            }
                Hello.FontSize += 2;
            Clicks++;
            Hello.Text = Clicks.ToString();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
        void GameOver()
        {
            if(firstCall)
            {
                firstCall = false;
                return;
            }
            if (Game)
            {
                Game = false;
                AddClick.IsEnabled = false;
                NewGame.Visibility = Visibility.Visible;
                Timer.RemoveAction(GameOver, 10000);
            }
            if (Clicks > BestResult)
            {
                BestResult = Clicks;
                Result.Text = BestResult.ToString();
                File.WriteAllText("..\\..\\Результаты\\Result.TXT", BestResult.ToString());
            }
        }
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            firstCall = true;
            Game = false;
            Clicks = 0;
            AddClick.IsEnabled = true;
            NewGame.Visibility = Visibility.Collapsed;
            Hello.FontSize = 70;
            Hello.Text = "0";
        }
    }
}