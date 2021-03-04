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
using GameMaps;
using GameMaps.Layouts;
using System.Windows.Shapes;

namespace Tower_Defenc
{
    /// <summary>b
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TimerController timer = new TimerController();
        UniversalMap_Wpf map;
        static public GameObject SelectedUnit;
        CellMapInfo cellMap = new CellMapInfo(31, 21, 50, 0);
        InventoryPanel ipan;
        GameObject basa;
        bool CanSpawn = true;
        static public List<GameObject> Allies = new List<GameObject>();
        List<GameObject> CreatedAnimation = new List<GameObject>();
        InventoryPanel UnitsPanel;
        int counter = 0;
        int money = 150;
        IGameScreenLayout lay;
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, cellMap);
            ipan = new InventoryPanel(map.Library, 40, 16);
            UnitsPanel = new InventoryPanel(map.Library, 120, 16);
            lay = LayoutsFactory.GetLayout(LayoutType.Vertical, this.Content);
            lay.SetBackground(Brushes.Wheat);
            lay.Attach(map, 0);
            lay.Attach(ipan, 1);
            lay.Attach(UnitsPanel, 1);
            AddPictures();
            ipan.AddItem("money", "money", money.ToString());
            UnitsPanel.AddItem("ЧИСТОТАНК", "Tank_Low_AllY", "Средний танк без поворота башни: 50");
            UnitsPanel.SetMouseClickHandler(BuildUnit);
            GameObject.map = map;
            basa = new GameObject("basa", "BASA", "basa");
            map.ContainerSetSize(basa.ContainerName, 100, 100);
            basa.SetCoordinate(map.XAbsolute / 2, map.YAbsolute / 2);
            timer.AddAction(GameCycle, 20);
            map.Mouse.SetMouseSingleLeftClickHandler(MapClick);
            //map.Keyboard.SetSingleKeyEventHandler(CheckKey);
        }
        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("background", "Сзади.png");
            map.Library.AddPicture("money", "money.png");
            map.Library.AddPicture("nothing", "nothing.png");
            map.Library.AddPicture("Tank_Low_AllY", "Танк с башней без поворота(ДОБРЫЙ средний).png");
            map.Library.AddPicture("basa", "base.png");
            map.Library.AddPicture("AllyScope", "Выбрал нашего.png");
            map.SetMapBackground("background");
            AddSetPictures("Fire Bolt", 6);
            AddSetPictures("ДЫМ", 4);
            CreateAnimation("Fire Bolt", 6, "Fire1");
        }
        void CreateAnimation(string PictureName, int FrameCount, string AnimationName)
        {
            string[] Frames = new string[FrameCount];
            AnimationDefinition F = new AnimationDefinition();
            for (int i = 1; i <= Frames.Length; i++)
            {
                Frames[i - 1] = PictureName + i;
            }
            F.AddEqualFrames(100, Frames);
            map.Library.AddAnimation(AnimationName, F);
        }
        void AddSetPictures(string BaseName, int FramesCount)
        {
            for (int i = 1; i <= FramesCount; i++)
            {
                map.Library.AddPicture(BaseName + i, BaseName + i + ".png");
            }
        }

        void MapClick(int x, int y, int Cx, int Cy)
        {
            if (SelectedUnit == null) return;
            SelectedUnit.TargetX = x;
            SelectedUnit.TargetY = y;
            SelectedUnit.NeedToMove = true;
            SelectedUnit.Speed = 1;
        }

        void BuildUnit(string UnitName)
        {
            if(!CanSpawn)
            {
                return;
            }
            counter++;
            if (UnitName == "ЧИСТОТАНК")
            {
                if(money >=  50)
                {
                    AddMoney(-50);
                    GameObject Tank = new GameObject("Tank_Low_AllY", "Tank_Low_ally" + counter.ToString(), "Tank_Low_ally", map.XAbsolute / 2, map.YAbsolute / 2, 60);
                    CreatedAnimation.Add(Tank);
                    Allies.Add(Tank);
                    Tank.SetHp(100);
                    /*map.ContainerSetFrame(Tank.ContainerName + "Anime", "Fire Bolt1");
                    map.ContainerSetMaxSide(Tank.ContainerName + "Anime", 100);
                    map.AnimationStart(Tank.ContainerName + "Anime", "Fire1", -1);*/
                }
            }
            // ДЗ. Сделать вторую проверку на второй юнит
        }

        void AddMoney(int NewMoney)
        {
            money = money + NewMoney;
            ipan.SetText("money", money.ToString());
        }

        void AnimatCreation()
        {
            for(int i = 0; i < CreatedAnimation.Count; i++)
            {
                var unit = CreatedAnimation[i];
                if (basa.X - unit.X < 100)
                {
                    unit.SetCoordinate(unit.X - 1, unit.Y);
                }
                else
                {
                    CreatedAnimation.Remove(unit);
                }
            }
        }

        void ChekSpawn()
        {
            for(int i = 0; i < Allies.Count; i++)
            {
                if(Allies[i].X < map.XAbsolute / 2 &&
                   Allies[i].X > map.XAbsolute / 2 - 150 &&
                   Allies[i].Y < map.YAbsolute / 2 + 50 &&
                   Allies[i].Y > map.YAbsolute / 2 - 50)
                   
                {
                    CanSpawn = false;
                    return;
                }
            }
            CanSpawn = true;
        }

        void GameCycle()
        {
            AnimatCreation();
            ChekSpawn();
            for(int i = 0; i < Allies.Count; i++)
            {
                Allies[i].Move();
            }
        }
    }
}