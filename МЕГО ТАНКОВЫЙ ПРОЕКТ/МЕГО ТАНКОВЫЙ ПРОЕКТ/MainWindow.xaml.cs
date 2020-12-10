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
        Image part3;
        Image part1;
        Image part2;
        string SelectedImage;
        public MainWindow()
        {
            InitializeComponent();
            ///////////////////////////////////////////////////////////////////////
            part3 = LoadImage(@"C:\Users\Admin\Documents\GitHub\GameProject\МЕГО ТАНКОВЫЙ ПРОЕКТ\МЕГО ТАНКОВЫЙ ПРОЕКТ\Images\Ходовая т - 26.png");
            part1 = LoadImage(@"C:\Users\Admin\Documents\GitHub\GameProject\МЕГО ТАНКОВЫЙ ПРОЕКТ\МЕГО ТАНКОВЫЙ ПРОЕКТ\Images\Башня т - 26.png");
            part2 = LoadImage(@"C:\Users\Admin\Documents\GitHub\GameProject\МЕГО ТАНКОВЫЙ ПРОЕКТ\МЕГО ТАНКОВЫЙ ПРОЕКТ\Images\Корпус т - 26.png");
            ///////////////////////////////////////////////////////////////////////
            part1.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            part1.Tag = "part1";
            SetCoordinate(part1, 0, 982);
            ////////////////////////////////////////////////////////////////////////
            part2.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            SetCoordinate(part2, 200, 982);
            part2.Tag = "part2";
            ////////////////////////////////////////////////////////////////////////
            SetCoordinate(part3, 350, 982);
            part3.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            part3.Tag = "part3";
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
        void SetCoordinate(Image image1, int x, int y)
        {
            Canvas.SetLeft(image1, x);
            Canvas.SetTop(image1, y);
        }

        private void desktop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            SelectedImage = image.Tag.ToString();
            // MessageBox.Show("ЛОЛ ТЫ СЛАВИЛ ПАСХАЛКУ ЕЁ ШАНС ВЫПАДЕНИЯ = 1к 933724723 ПРОЦЕНЬТОВ!!!!!!!!!!!!!!!!!!! ВАУУУУУУУУ. нажми ок будет классс ВАМ ПРИДЁТ 1000000 руб через 2 часа!!!!!!!");
            //MessageBox.Show("Если ты ткнул на ОК то ваш компьютер захватила программа VILKOM ВАШИ ДАННЫЕ ВМЕСТЕ С ПРОФИЛЕМ УНИЧТОЖАТ!!!!!!!!!!! ЧЕРЕЗ 10 минут. ЗАХЫВАТ ПОШЁЛ!!!!!!!!!!!! Если вы ткн1ёте на экран то всё увидете цыфры это кодовые имена ХАКЕРОВ которые захватывают ваш компьютер ХАХАХА!!!");
        }

        private void desktop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(desktop);
            string s = p.X + "." + p.Y;
            // MessageBox.Show(s);
            Image img = GetImageByName();
            SetCoordinate(img, (int)(p.X), (int)(p.Y));
        }
        Image GetImageByName()
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
        }

        private void desktop_KeyDown(object sender, KeyEventArgs e)
        {
            Point p;
            Image img = GetImageByName();
            switch (e.Key)
            {
                case Key.Up: 
                    p = img.PointToScreen(new Point (0,0));
                    break;
            }
        }
    }
}