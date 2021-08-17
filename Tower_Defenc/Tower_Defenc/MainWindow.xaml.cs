﻿using System;
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
        int CenterX, CenterY;
        Random r = new Random();
        TimerController timer = new TimerController();
        UniversalMap_Wpf map;
        static public GameObject SelectedEnemyUnit;
        static public List<GameObject> obstacle = new List<GameObject>();
        SimpleTextBox TextTimer = new SimpleTextBox();
        static public GameObject SelectedUnit; // Переменная для хранения выбранного юнита.
        CellMapInfo cellMap = new CellMapInfo(100, 100, 50, 0);
        InventoryPanel ipan;
        public static int ClickCount = 0;
        int scrollX;
        int scrollY;
        GameObject basa;
        bool CanSpawn = true;
        List<GameObject> Enemis = new List<GameObject>();
        static public List<GameObject> Allies = new List<GameObject>();
        List<GameObject> AlliesShots = new List<GameObject>();
        List<GameObject> EnemisShots = new List<GameObject>();
        List<GameObject> CreatedAnimation = new List<GameObject>();
        Wave[] waves = new Wave[1]; 
        InventoryPanel UnitsPanel;
        int countdown = 30;
        int counter = 0;
        int money = 150;
        int WaveNumber = 0;
        IGameScreenLayout lay;
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, cellMap);
            CenterX = (int)map.Canvas.Width / 2;
            CenterY = (int)map.Canvas.Height / 2;
            scrollX = -CenterX + 800;
            scrollY = -CenterY + 400;
            map.Canvas.Margin = new Thickness(scrollX, scrollY, 0, 0);
            ipan = new InventoryPanel(map.Library, 40, 16);
            UnitsPanel = new InventoryPanel(map.Library, 120, 16);
            lay = LayoutsFactory.GetLayout(LayoutType.Vertical, this.Content);
            lay.SetBackground(Brushes.Wheat);
            lay.Attach(map, 0);
            lay.Attach(ipan, 1);
            lay.Attach(UnitsPanel, 1);
            lay.Attach(TextTimer, 1);
            AddPictures();
            ipan.AddItem("money", "money", money.ToString());
            UnitsPanel.AddItem("ЧИСТОТАНК", "Tank_Low_AllY", "Средний танк без поворота башни: 50. ИМЯ: ЖАР");
            UnitsPanel.SetMouseClickHandler(BuildUnit);
            UnitsPanel.AddItem("ПУЛЕМЁТНИК", "TankMashingan_Medium_ALLY", "Малый танк без поворота башни: 100. ИМЯ: БАХ");
            GameObject.map = map;
            basa = new GameObject("basa", "BASA", "basa");
            map.ContainerSetSize(basa.ContainerName, 100, 100);
            basa.SetCoordinate(CenterX, CenterY);
            timer.AddAction(GameCycle, 20);
            timer.AddAction(Countdown, 1000);
            TextTimer.TextBox.IsEnabled = false;
            map.Mouse.SetMouseSingleLeftClickHandler(MapClick);
            CreateWaves();
            timer.AddAction(CheckScroll, 12);
            //map.Keyboard.SetSingleKeyEventHandler(CheckKey);
        }

        void CheckScroll()
        {
            if (map.Keyboard.IsKeyPressed(Key.Up))
            {
                scrollY = scrollY - 5;
                map.Canvas.Margin = new Thickness(scrollX,scrollY, 0, 0);
            }

            if (map.Keyboard.IsKeyPressed(Key.Down))
            {
                scrollY = scrollY + 5;
                map.Canvas.Margin = new Thickness(scrollX, scrollY, 0, 0);
            }

            if (map.Keyboard.IsKeyPressed(Key.Right))
            {
                scrollX = scrollX + 5;
                map.Canvas.Margin = new Thickness(scrollX, scrollY, 0, 0);
            }

            if (map.Keyboard.IsKeyPressed(Key.Left))
            {
                scrollX = scrollX - 5;
                map.Canvas.Margin = new Thickness(scrollX, scrollY, 0, 0);
            }
        }

        void AddPictures()
        {
            map.Library.ImagesFolder = new PathInfo { Path = "..\\..\\images", Type = PathType.Relative };
            map.Library.AddPicture("background", "Сзади.png");
            map.Library.AddPicture("money", "money.png");
            map.Library.AddPicture("nothing", "nothing.png");
            map.Library.AddPicture("Tank_Low_AllY", "Танк с башней без поворота(ДОБРЫЙ средний).png");
            map.Library.AddPicture("BulletAllies", "СнарядДобрый.png");
            map.Library.AddPicture("basa", "base.png");
            map.Library.AddPicture("snow", "SONOW.png");
            map.Library.AddPicture("TankMashingan_Medium_ALLY", "Танк с башней без поворота(ДОБРЫЙ СЛАБЫЙПУЛЕМЁТНЫЙ).png");
            map.Library.AddPicture("MarkEnemy", "EnemyDetected.png");
            map.Library.AddPicture("TankMashingan_Medium_ALLY_destroid", "Танк подорвали(второй).png");
            map.Library.AddPicture("AllyScope", "Выбрал нашего.png");
            map.Library.AddPicture("EnemyLOW", "Враг(самаходка).png");
            map.Library.AddPicture("Destroyed_Tank_Low_ALLY", "Танк подорвали(первый).png");
            map.SetMapBackground("background");
            AddSetPictures("Fire Bolt", 6);
            AddSetPictures("ДЫМ", 4);
            AddSetPictures("exp", 10);
            CreateAnimation("Fire Bolt", 6, "Fire1");
            CreateAnimation("exp", 10, "Explosion_Collision");
            /////////////////////////////////////////////////////// т
            /////////////////////////////////////////////////////// е
            /////////////////////////////////////////////////////// к
            /////////////////////////////////////////////////////// с
            /////////////////////////////////////////////////////// т
            /////////////////////////////////////////////////////// у
            /////////////////////////////////////////////////////// р
            /////////////////////////////////////////////////////// а
            map.Library.AddContainer("TextureUp", "snow", ContainerType.AutosizedSingleImage);
            //map.ContainerSetSize("TextureUp", 5000, 1000);
            map.ContainerSetMaxSide("TextureUp", 2000 );
            map.ContainerSetCoordinate("TextureUp", 2500, 1000);
            //map.Library.AddContainer("MainTexture", "background", ContainerType.SingleImage);
            //map.ContainerSetSize("MainTexture", 400, 400);
            //map.ContainerSetCoordinate("MainTexture", 0, 0);
        }

        void CreateWaves()
        {
            waves[0] = new Wave();
            waves[0].Units = new List<WaveUnit>();
            WaveUnit unit = new WaveUnit();
            unit.UnitName = "EnemyLOW";
            unit.UnitCount = 4;
            waves[0].Units.Add(unit);
        }

        void StartWave(int number)
        {
            for(int i = 0; i < waves[number].Units.Count; i++)
                {
                switch (waves[number].Units[i].UnitName)
                {
                    case "EnemyLOW":
                        SpaunEnemyLow(waves[number].Units[i].UnitCount);
                        break;
                }
                // wawes[number] - Это обьект типа Wawe который хранит информацию о волне.
            }
        }

        void SpaunEnemyLow(int UnitNumber)
        {
            for (int i = 0; i < UnitNumber; i++)
            {
                counter++;
                GameObject Enemy = new GameObject("EnemyLOW", "Enemy1" + counter.ToString(), "EnemyLOW");
                int x = CenterX, y = CenterY;
                Enemy.TargetObject = new GameObject();
                Enemy.TargetObject.X = CenterX;
                Enemy.TargetObject.Y = CenterY;
                while (x > CenterX - 900 && x < CenterX + 900 && y > CenterY - 600 && y < CenterY + 600)
                {
                    x = r.Next(0, CenterX * 2);
                    y = r.Next(0, CenterY * 2);
                }
                Enemy.SetCoordinate(x, y);
                Enemy.SetAngle((int)GameMath.GetAngleOfVector(CenterX - x, CenterY - y));
                map.ContainerSetMaxSide(Enemy.ContainerName, 72);
                Enemy.Speed = 1;
                obstacle.Add(Enemy);
                Enemy.SetHp(72);
                Enemy.Range = 150;
                Enemis.Add(Enemy);
                Enemy.NeedToMove = true;
                Enemy.NeedToRotate = true;
                Enemy.SubdivisionNumber = 1;
                /*
                  Баг "ХАКЕР"
                    1 - Подождите 30 секунд(после запуска игры);
                    2 - Наведите на врага мышь и нажмите леву кнопку мыши; 
                    3 - ВЫ МОЖЕТЕ УПРАВЛЯТЬ ВРАГОМ!!!!!!!!!!;
               -------------------------------------------------------------------------------------------------------------
                   Решение бага "ХАКЕР"
                */
            }
        }

        void Countdown()
        {
            countdown--;
            if (countdown == 0)
            {
                timer.RemoveAction(Countdown, 1000);
                StartWave(WaveNumber);
                WaveNumber++;
            }
            TextTimer.TextBox.Text = "АТАКА ГОРГОВ НАЧНЁТСЯ ЧЕРЕЗ: " + countdown.ToString();
            TextTimer.TextBox.FontSize = 20;
            TextTimer.TextBox.Background = Brushes.DarkRed;
        }

        void CreateAnimation(string PictureName, int FrameCount, string AnimationName, string LastFrame = "nothing")
        {
            string[] Frames = new string[FrameCount];
            AnimationDefinition F = new AnimationDefinition();
            for (int i = 1; i <= Frames.Length; i++)
            {
                Frames[i - 1] = PictureName + i;
            }
            F.AddEqualFrames(100, Frames);
            F.AddFrame(1, LastFrame);
            F.LastFrame = LastFrame;
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
            if (SelectedUnit.HP == 0) return;
            if (ClickCount > 0)
            {
                SelectedUnit.NeedToRotate = true;
                if(map.Keyboard.IsKeyPressed(Key.Space))
                {
                    SelectedUnit.NeedToMove = true;
                }
                SelectedUnit.TargetObject = SelectedEnemyUnit;
            }

            else 
            {
                SelectedUnit.TargetObject = new GameObject();
                SelectedUnit.TargetObject.X = x;
                SelectedUnit.TargetObject.Y = y;
                SelectedUnit.NeedToMove = true;
                SelectedUnit.NeedToRotate = true;
                SelectedUnit.Speed = 1;
            }
        }

        void BuildUnit(string UnitName)
        {
            if (!CanSpawn)
            {
                return;
            }
            counter++;
            switch (UnitName)
            {   
                case "ЧИСТОТАНК":
                    if (money >= 50)
                    {
                        AddMoney(-50);
                        GameObject Tank = new GameObject("Tank_Low_AllY", "Tank_Low_ally" + counter.ToString(), "Tank_Low_ally", CenterX, CenterY, 70);
                        CreatedAnimation.Add(Tank);
                        Allies.Add(Tank);
                        Tank.SetHp(100);
                        Tank.SubdivisionNumber = 0;
                        /*map.ContainerSetFrame(Tank.ContainerName + "Anime", "Fire Bolt1");
                        map.ContainerSetMaxSide(Tank.ContainerName + "Anime", 100);
                        map.AnimationStart(Tank.ContainerName + "Anime", "Fire1", -1);*/
                        obstacle.Add(Tank);
                    }
                    break;
                case "ПУЛЕМЁТНИК":
                    if(money >= 100)
                    {
                        AddMoney(-100);
                        GameObject Tank = new GameObject("TankMashingan_Medium_ALLY", "TankMashingan_Medium_ALLY" + counter.ToString(), "TankMashingan_Medium_ALLY", CenterX, CenterY, 55);
                        CreatedAnimation.Add(Tank);
                        Tank.SetHp(120);
                        Allies.Add(Tank);
                        Tank.SubdivisionNumber = 0;
                        obstacle.Add(Tank);
                    }
                    break;

                /*case ""
                    if (money >= 100)
                    {

                    }
                    break;*/
            }
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
                if(Allies[i].X < CenterX &&
                   Allies[i].X > CenterX - 150 &&
                   Allies[i].Y < CenterX + 50 &&
                   Allies[i].Y > CenterY - 50)
                   
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
            for (int i = 0; i < Allies.Count; i++)
            {
                Allies[i].Rotate();
                Allies[i].Move();
                Allies[i].PerformAction();
            }
            for(int i = 0; i < Enemis.Count; i++)
            {
                Enemis[i].Move();
                Enemis[i].Rotate();
                Enemis[i].CheckMaxCounter();
            }
            ClickCount--;
        }
    }
}