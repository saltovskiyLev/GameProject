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

namespace МЕГО_ТАНКОВЫЙ_ПРОЕКТ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Image image;
        bool CanvasUp = false;
        double ImgX;
        double ImgY;
        Image[] parts = new Image[6];
        string[] images = new string[6];
        public MainWindow()
        {
            InitializeComponent();
            images[0] = @"C:\Users\Admin\Documents\GitHub\GameProject\МЕГО ТАНКОВЫЙ ПРОЕКТ\МЕГО ТАНКОВЫЙ ПРОЕКТ\Images\Башня т - 26.png";
            images[1] = @"C:\Users\Admin\Documents\GitHub\GameProject\МЕГО ТАНКОВЫЙ ПРОЕКТ\МЕГО ТАНКОВЫЙ ПРОЕКТ\Images\Корпус т - 26.png";
            images[2] = @"C:\Users\Admin\Documents\GitHub\GameProject\МЕГО ТАНКОВЫЙ ПРОЕКТ\МЕГО ТАНКОВЫЙ ПРОЕКТ\Images\Ходовая т - 26.png";
            images[3] = @"C:\Users\Admin\Documents\GitHub\GameProject\МЕГО ТАНКОВЫЙ ПРОЕКТ\МЕГО ТАНКОВЫЙ ПРОЕКТ\Images\Башня т - 34.png";
            images[4] = @"C:\Users\Admin\Documents\GitHub\GameProject\МЕГО ТАНКОВЫЙ ПРОЕКТ\МЕГО ТАНКОВЫЙ ПРОЕКТ\Images\Корпус т - 34.png";
            images[5] = @"C:\Users\Admin\Documents\GitHub\GameProject\МЕГО ТАНКОВЫЙ ПРОЕКТ\МЕГО ТАНКОВЫЙ ПРОЕКТ\Images\Ходовая т - 34.png";
            ////////////////////////////////////////////////////////////////////////
            BTN_RIGHT.Content = ">";
            BTN_LEFT.Content = "<";
            for (int i = 0; i < images.Length; i++)
            {
                parts[i] = LoadImage(images[i]);
                parts[i].MouseLeftButtonDown += Image_MouseLeftButtonDown;
            }
            SetCoordinate(parts[0], 0, 982);
            SetCoordinate(parts[1], 200, 982);
            SetCoordinate(parts[2], 350, 982);
            SetCoordinate(parts[3], 450, 1000);
            SetCoordinate(parts[4], 560, 200);
            SetCoordinate(parts[5], 720, 100);
        }
        Image LoadImage(string path)
        {
            Image part1 = new Image();
            Uri PATH1 = new Uri(path);
            BitmapImage PIC1 = new BitmapImage(PATH1);
            part1.Source = PIC1;
            desktop.Children.Add(part1);
            return part1;
        }
        void SetCoordinate(Image image1, double x, double y)
        {
            Canvas.SetLeft(image1, x);
            Canvas.SetTop(image1, y);
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            image = (Image)sender;
            //MessageBox.Show("ЛОЛ ТЫ СЛАВИЛ ПАСХАЛКУ ЕЁ ШАНС ВЫПАДЕНИЯ = 1к 933724723 ПРОЦЕНЬТОВ!!!!!!!!!!!!!!!!!!! ВАУУУУУУУУ. нажми ок будет классс ВАМ ПРИДЁТ 1000000 руб через 2 часа!!!!!!!");
            //MessageBox.Show("Если ты ткнул на ОК то ваш компьютер захватила программа "VILKOM" ВАШИ ДАННЫЕ ВМЕСТЕ С ПРОФИЛЕМ УНИЧТОЖАТ!!!!!!!!!!! ЧЕРЕЗ 10 минут. ЗАХЫВАТ ПОШЁЛ!!!!!!!!!!!! Если вы ткн1ёте на экран то всё увидете цыфры это кодовые имена ХАКЕРОВ которые захватывают ваш компьютер ХАХАХА!!!");
        }

        private void desktop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(desktop);
            string s = p.X + "." + p.Y;
            // MessageBox.Show(s);
            if (image != null)
            {
                //if (p.X =)
                SetCoordinate(image, (int)(p.X), (int)(p.Y));
                ImgX = p.X;
                ImgY = p.Y;
                //Point point1 = img.PointFromScreen(new Point(0,0));
            }
        }
        /*Image GetImageByName()
        {
            Image img = null;
            switch (SelectedImage)
            {
                case "part1":
                    img = part1;
                    break;
                case "part2":
                    img = part2;
                    break;
                case "part3":
                    img = part3;
                    break;
            }
            return img;
        }*/

        /*private void desktop_KeyDown(object sender, KeyEventArgs e)
        {
            Point p;
            Image img = ();
            switch (e.Key)
            {
                case Key.Up: 
                    p = img.PointToScreen(new Point (0,0));
                    break;
            }
        }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CanvasUp == false)
            {
                CanvasUp = true;
                desktop.Margin = new Thickness(0, -500, 0, 0);
            }
            else
            {
                desktop.Margin = new Thickness(0, 0, 0, 0);
                CanvasUp = false;
            }
        }

        private void BTN_UP_Click(object sender, RoutedEventArgs e)
        {
            ImgY--;
            SetCoordinate(image, ImgX, ImgY);
        }

        private void BTN_DOWN_Click(object sender, RoutedEventArgs e)
        {
            ImgY++;
            SetCoordinate(image, ImgX, ImgY);
        }

        private void BTN_RIGHT_Click(object sender, RoutedEventArgs e)
        {
            ImgX++;
            SetCoordinate(image, ImgX, ImgY);
        }

        private void BTN_LEFT_Click(object sender, RoutedEventArgs e)
        {
            ImgX--;
            SetCoordinate(image, ImgX, ImgY);
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(image != null) 
            {
                desktop.Children.Remove(image);
                /*for(int i = 0; i < desktop.Children.Count; i++)
                {
                    
                }*/
            }
        }
    }
}