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
using GameMaps.Layouts;

namespace GameProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IGameScreenLayout Lay; // Разметка окна (определяет, как именно разместятся карта и область меню)
        UniversalMap_Wpf Map; // Карта
        CellMapInfo MapInfo; // Параметры карты
        InventoryPanel Items; // Панель предметов
        TextArea_Vertical Info; // Место для текстовой информации

        public MainWindow()
        {
            #region Подготовка карты и панели меню
            InitializeComponent();
            // Определяем вид разметки окна: область карты слева и меню справа
            Lay = LayoutsFactory.GetLayout(LayoutType.Vertical, this.Content);

            // Определяем параметры карты: количество клеток по горизонтали и вертикали, размер клетки, 
            // ширина декоративной рамки вокруг карты
            MapInfo = new CellMapInfo(10, 10, 50, 5);

            // Создаем карту и размещаем его в окне программы
            Map = MapCreator.GetUniversalMap(this, MapInfo);
            Lay.Attach(Map, 0);
            Map.DrawGrid(); // выводим сетку

            // Указываем путь к папке с картинками 
            Map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };

            // Создаем панель инвентаря и размещаем ее в меню
            Items = new InventoryPanel(Map.Library, Map.CellSize);
            Lay.Attach(Items, 1);
            Items.SetBackground(Brushes.Wheat);

            // Создаем текстовую панель и размещаем ее в меню
            Info = new TextArea_Vertical();
            Lay.Attach(Info, 1);

            // определяем функцию, которая будет вызвана при нажатии на клавишу
            //Map.Keyboard.SetSingleKeyEventHandler(CheckKey);
            #endregion

            //=======================================================================
            //                         Пример кода

            Map.Library.AddPicture("red", "gem_red.png");
            Map.Library.AddPicture("stone", "stone.png");


            //=======================================================================
            // Задания делаем в отдельной функции
            // Здесь вызываем функции Hometask1, Hometask2, Hometask3, чтобы посмотреть результат

            Hometask1();
            //Hometask2();
            //Hometask3();
        }

        void Hometask1()
        {
            // Здесь пишем код для домашнего задания:
            for(int i = 1; i <= 8; i++)
            {
                Map.DrawInCell("stone", i, i);
                Map.DrawInCell("stone", i + 1, i);
            }
        }

        void Hometask2()
        {
            // Здесь пишем код для домашнего задания:

        }

        void Hometask3()
        {
            // Здесь пишем код для домашнего задания:

        }
    }
}
