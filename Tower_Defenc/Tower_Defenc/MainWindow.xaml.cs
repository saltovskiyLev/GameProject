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
using Newtonsoft.Json;
using System.IO;
using GameMaps.Layouts;
using System.Windows.Shapes;
using MenuPanel_For_TowerDefenc_;

namespace Tower_Defenc
{
    /// <summary>b
    /// Interaction logic for MainWindow.xaml
    /// </summary> 

    // класс I(добавление переменных и тд.)
    public partial class MainWindow : Window 
    {
        static public List<GameObject> Crystals = new List<GameObject>();
        static public List<GameObject> Enemis = new List<GameObject>();
        static public List<GameObject> Allies = new List<GameObject>();
        static public List<GameObject> AlliesShots = new List<GameObject>();
        static public List<GameObject> EnemisShots = new List<GameObject>();
        //
        int CenterX, CenterY;
        int scrollX;
        int scrollY;
        //
        static public UniversalMap_Wpf map;
        CellMapInfo cellMap = new CellMapInfo(100, 100, 50, 0);
        //
        List<GameObject> Mins = new List<GameObject>();
        List<GameObject> AmmoDeports = new List<GameObject>();
        List<GameObject> Artillery = new List<GameObject>();
        //
        static public GameObject SelectedEnemyUnit;
        static public GameObject SelectedUnit; // Переменная для хранения выбранного юнита.
        // остальное <ниже>
        Random r = new Random();
        static TextBlock tbShowMoney;
        MenuPanel UnitsMenu = new MenuPanel();
        Dictionary<AllyUnits, int> Cost = new Dictionary<AllyUnits, int>();
        bool CanSpawn = true;
        int ChargeArtilleryReload = 0;
        int ArtylleryState = 0;
        // 0 - заряжается
        // 1 - готово к выстрелу
        // 2 - стреляет
        int ArtilleryLoad = 4000;
        //остальное <выше>
        GameObject basa;
        GameObject BaseEnemy;
        //
        TimerController timer = new TimerController();
        SimpleTextBox TextTimer = new SimpleTextBox();
        //
        static public List<GameObject> obstacle = new List<GameObject>();
        List<GameObject> CreatedAnimation = new List<GameObject>();
        //
        Wave[] waves = new Wave[1];
        InventoryPanel UnitsPanel;
        //
        int CursorMode = 1;
        int countdown = 12;
        int counter = 0;
        public static int ClickCount = 0;
        static public int money = 5000;
        int WaveNumber = 0;
        int PlaceHolderAngle;
        //
        Rectangle PlaceHolder = new Rectangle();
        IGameScreenLayout lay;
        // Функция I(вызов функций, добавление объектов в панели, создание карты и тд.)
        public MainWindow()
        {
            InitializeComponent();
            map = MapCreator.GetUniversalMap(this, cellMap);
            CenterX = (int)map.Canvas.Width / 2;
            CenterY = (int)map.Canvas.Height / 2;
            scrollX = -CenterX + 800;
            scrollY = -CenterY + 400;
            map.Canvas.Margin = new Thickness(scrollX, scrollY, 0, 0);
            UnitsPanel = new InventoryPanel(map.Library, 120, 16);
            lay = LayoutsFactory.GetLayout(LayoutType.SingleZone, GridMap);
            lay.SetBackground(Brushes.Wheat);
            lay.Attach(map, 0);
            lay.Attach(UnitsPanel, 1);
            lay.Attach(TextTimer, 1);
            AddPictures();
            SetCost();
            CreateUsitsPanel();
            GameObject.map = map;
            basa = new GameObject("basa", "BASA", "basa");
            timer.AddAction(GameCycle, 20);
            timer.AddAction(Countdown, 1000);
            TextTimer.TextBox.IsEnabled = false;
            timer.AddAction(CrystalSpawn, 1000);
            CreateBases();
            map.Mouse.SetMouseSingleLeftClickHandler(MapClick);
            tbShowMoney = tbMoney;
            CreateWaves();
            GameObject BossEnemy = new GameObject();
            timer.AddAction(CheckScroll, 12);
            timer.AddAction(RotateObject, 12);
            map.Canvas.Children.Add(PlaceHolder);
            PlaceHolder.Fill = Brushes.Gray;
            PlaceHolder.Opacity = 0.5;
            map.Keyboard.SetSingleKeyEventHandler(CheckKey);
            AddMoney(0);
            CreateMenu();
            GameObject g = JsonGameObjects.GetJsonGO(@"C:\Users\Admin\Documents\GitHub\GameProject\Tower_Defenc\Tower_Defenc\Документы\параметры игровых объектов\ПротивникМалый.json");
            g.SetCoordinate(700, 500);
        }

        void SetCost()
        {
            Cost.Add(AllyUnits.БОЕЗАПАС, 500);
            Cost.Add(AllyUnits.ПУЛЕМЁТНИК, 100);
            Cost.Add(AllyUnits.ЧИСТОТАНК, 50);
            Cost.Add(AllyUnits.Cборщик_Ресурсов, 25);
            Cost.Add(AllyUnits.МИНА, 100);
            Cost.Add(AllyUnits.Арта, 200);
        }

        void CreateUsitsPanel()
        {
            UnitsPanel.AddItem(AllyUnits.Cборщик_Ресурсов.ToString(), "scavenger",
    "Он будет делать ваши деньги: " + Cost[AllyUnits.Cборщик_Ресурсов] + " Имя: РООБ");
            UnitsPanel.AddItem(AllyUnits.ЧИСТОТАНК.ToString(), "Tank_Low_AllY",
                "Средний танк без поворота башни: " + Cost[AllyUnits.ЧИСТОТАНК] + " ЖАР");
            UnitsPanel.AddItem(AllyUnits.ПУЛЕМЁТНИК.ToString(), "ПУЛЕМЁТНИК",
                "Малый танк без поворота башни: " + Cost[AllyUnits.ПУЛЕМЁТНИК] + " ИМЯ: БАХ");
            UnitsPanel.AddItem(AllyUnits.БОЕЗАПАС.ToString(), "warehouse",
                "Домик с Боезопасом: " + Cost[AllyUnits.БОЕЗАПАС] + " Имя: BVZ");
            UnitsPanel.AddItem(AllyUnits.МИНА.ToString(), "mine",
                "МИНА ВЗОРВЁТ ВСЕХ: " + Cost[AllyUnits.МИНА] + " Имя: БАБАХ");
            UnitsPanel.AddItem(AllyUnits.Арта.ToString(), "Artillery",
                "СЕЙЧАС БУДЕТ ЖАРКО: " + Cost[AllyUnits.Арта] + " Имя: Арта");
            // выше идёт добавление танков и зданиний в UnitsPanel
            UnitsPanel.SetMouseClickHandler(Build);
        }

        void CreateBases()
        {

            // работа с базой врага //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            BaseEnemy = new GameObject("EnemyBase", "BaseEnemy", "EnemyBase");
            map.ContainerSetMaxSide("BaseEnemy", 100);
            BaseEnemy.SetCoordinate(240, 300);
            BaseEnemy.SetHp(700);
            Enemis.Add(BaseEnemy);
            BaseEnemy.destroyedImage = "EnemyBase(D)";
            BaseEnemy.destroyedAmimathion = "Explosion_Collision";
            BaseEnemy.NeedToMove = false;
            BaseEnemy.NeedToRotate = false;
            BaseEnemy.SubdivisionNumber = 1;
            // работа с базой врага //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // работа с нашей базой
            map.ContainerSetSize(basa.ContainerName, 100, 100);
            basa.SetCoordinate(CenterX, CenterY);
            Allies.Add(basa);
            basa.destroyedImage = "DeathBase";
            basa.destroyedAmimathion = "Explosion_Collision";
            basa.NeedToMove = false;
            basa.NeedToRotate = false;
            basa.SubdivisionNumber = 0;
            basa.SetHp(120);
            basa.ContainerHeight = 100;
            basa.ContainerWidth = 100;
            map.ContainerSetIndents(basa.ContainerName, 27, 27);
        }

        void CheckEnemyBase()
        {
            for (int i = 0; i < Allies.Count; i++)
            {
                if (map.CollisionContainers(Allies[i].ContainerName, BaseEnemy.ContainerName))
                {
                    Allies[i].RemoveAction("move");
                    GameObject Point = new GameObject();
                    Point.SetCoordinate(
                    Allies[i].X + 500 * Math.Cos(GameMath.DegreesToRad(Allies[i].Angle + 180)),
                    Allies[i].Y + 500 * Math.Sin(GameMath.DegreesToRad(Allies[i].Angle + 180)));
                    MoveToTarget move = new MoveToTarget("move", Allies[i], Point);
                    Allies[i].Actions.Add(move);
                }
            }
        }

        void CheckScroll()
        {
            if (map.Keyboard.IsKeyPressed(Key.Up))
            {
                scrollY = scrollY - 5;
                map.Canvas.Margin = new Thickness(scrollX, scrollY, 0, 0);
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
            map.Library.AddPicture("DeathBase", "DeathBase.png");
            map.Library.AddPicture("money", "money.png");
            map.Library.AddPicture("nothing", "nothing.png");
            map.Library.AddPicture("warehouse", "i.png");
            map.Library.AddPicture("Artillery", "Artillery.png");
            map.Library.AddPicture("Tank_Low_AllY", "Танк с башней без поворота(ДОБРЫЙ средний).png");
            map.Library.AddPicture("BulletAllies", "СнарядДобрый.png");
            map.Library.AddPicture("basa", "base.png");
            map.Library.AddPicture("scavenger", "flyerSand4.png");
            map.Library.AddPicture("towerRed", "towerRed1.png");
            map.Library.AddPicture("platformRed4", "platformRed4.png");
            map.Library.AddPicture("RAKETA", "MissileRed1_.png");
            map.Library.AddPicture("snow", "SONOW.png");
            map.Library.AddPicture("crystal", "сristal.png");
            map.Library.AddPicture("MarkEnemy", "EnemyDetected.png");
            map.Library.AddPicture("TankMashingan_Medium_ALLY_destroid", "Танк подорвали(второй).png");
            map.Library.AddPicture("AllyScope", "Выбрал нашего.png");
            map.Library.AddPicture("ПУЛЕМЁТНИК", "Танк с башней без поворота(ДОБРЫЙ СЛАБЫЙПУЛЕМЁТНЫЙ).png");
            map.Library.AddPicture("EnemyBase", "scr_2.jpg");
            map.Library.AddPicture("Арта", "P2.png");
            map.Library.AddPicture("mine", "mine.png");
            map.Library.AddPicture("EnemyBase(D)", "scr_2(B).png");
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
            map.ContainerSetMaxSide("TextureUp", 2000);
            map.ContainerSetCoordinate("TextureUp", 2500, 1000);
            //map.Library.AddContainer("MainTexture", "background", ContainerType.SingleImage);
            //map.ContainerSetSize("MainTexture", 400, 400);
            //map.ContainerSetCoordinate("MainTexture", 0, 0);
        }

        void RandomEnemy()
        {
            if (r.Next(0, 100) < 100)
            {

                SpaunEnemy("EnemyBoss", 1);
            }
        }

        GameObject GetEnemy(string UnitType, string picture1, string picture2 = "")
        {
            counter++;
            GameObject Enemy;
            if (picture2 == "")
            {
                Enemy = new GameObject(picture1, "Enemy1" + counter.ToString(), UnitType);
            }
            else
            {
                Enemy = new GameObject(picture1, picture2, "Enemy1" + counter.ToString(), UnitType);
            }
            int x = CenterX, y = CenterY;
            Enemy.mode = 2;

            Enemy.TargetObject = new GameObject();
            Enemy.TargetObject.SetCoordinate(CenterX, CenterY);

            MoveToTarget move = new MoveToTarget("move", Enemy, Enemy.TargetObject, 320);
            Enemy.Actions.Add(move);
            while (x > CenterX - 900 && x < CenterX + 900 && y > CenterY - 600 && y < CenterY + 600)
            {
                x = r.Next(0, CenterX * 2);
                y = r.Next(0, CenterY * 2);
            }
            Enemy.SetCoordinate(x, y);
            Enemy.SetAngle((int)GameMath.GetAngleOfVector(CenterX - x, CenterY - y));
            obstacle.Add(Enemy);
            Enemy.targets = Allies;
            Enemis.Add(Enemy);

            Rotate rotate = new Rotate("rotate", Enemy, Enemy.TargetObject);
            Enemy.Actions.Add(rotate);

            Enemy.NeedToMove = true;
            Enemy.NeedToRotate = true;
            Enemy.SubdivisionNumber = 1;
            Enemy.destroyedImage = "Destroyed_Tank_Low_ALLY";
            return Enemy;
        }

        void CrystalSpawn()
        {
            int x = CenterX, y = CenterY;
            while (x > CenterX - 900 && x < CenterX + 900 && y > CenterY - 600 && y < CenterY + 600)
            {
                x = r.Next(0, CenterX * 2);
                y = r.Next(0, CenterY * 2);
            }
            GameObject crystal = new GameObject("crystal", "crystal" + counter, "crystal", x, y, 65);
            Crystals.Add(crystal);
            counter++;
        }

        // 
        void RotateObject()
        {
            if (CursorMode != 2) return;
            if(map.Keyboard.IsKeyPressed(Key.D9))
            {
                SetPlaceHolderAngle(PlaceHolderAngle + 5);
            }

            if (map.Keyboard.IsKeyPressed(Key.D0))
            {
                SetPlaceHolderAngle(PlaceHolderAngle - 5);
            }
        }

        void CreateMenu()
        {
            menuPanel.Children.Add(UnitsMenu);

            UnitsMenu.AddTab("Танки");
            UnitsMenu.AddTab("Здания");
            UnitsMenu.CreateNewItem("Малый танк\nбез поворота\nбашни: 50", @"C:\Users\Admin\Documents\GitHub\GameProject\Tower_Defenc\Tower_Defenc\images\Танк с башней без поворота(ДОБРЫЙ средний).png", "ЧИСТОТАНК", "Танки", Build);
            UnitsMenu.CreateNewItem("сборщик ресурсов: 100", @"C:\Users\Admin\Documents\GitHub\GameProject\Tower_Defenc\Tower_Defenc\images\flyerSand4.png", "Cборщик_Ресурсов", "Танки", Build);
            UnitsMenu.CreateNewItem("средний танк без поворота\nбашни: 100", @"C:\Users\Admin\Documents\GitHub\GameProject\Tower_Defenc\Tower_Defenc\images\Танк с башней без поворота(ДОБРЫЙ СЛАБЫЙПУЛЕМЁТНЫЙ).png", "ПУЛЕМЁТНИК", "Танки", Build);
            //UnitsMenu.
            UnitsMenu.CreateNewItem("Арта: 200", @"C:\Users\Admin\Documents\GitHub\GameProject\Tower_Defenc\Tower_Defenc\images\Artillery.png", "Арта", "Здания", Build);
            UnitsMenu.CreateNewItem("Дом со снарядами: 500", @"C:\Users\Admin\Documents\GitHub\GameProject\Tower_Defenc\Tower_Defenc\images\i.png", "БОЕЗАПАС", "Здания", Build);
            UnitsMenu.CreateNewItem("Мина танковая: 100", @"C:\Users\Admin\Documents\GitHub\GameProject\Tower_Defenc\Tower_Defenc\images\mine.png", "МИНА", "Здания", Build);
        }

        void CreateWaves()
        {
            waves[0] = new Wave();
            waves[0].Units = new List<WaveUnit>();
            WaveUnit unit = new WaveUnit();
            unit.UnitName = "EnemyLOW";
            unit.UnitCount = 3;
            waves[0].Units.Add(unit);
        }

        void StartWave(int number)
        {
            RandomEnemy();
            for (int i = 0; i < waves[number].Units.Count; i++)
            {
                switch (waves[number].Units[i].UnitName)
                {
                    case "EnemyLOW":
                        SpaunEnemy("EnemyLOW", waves[number].Units[i].UnitCount);
                        break;
                }
                // wawes[number] - Это обьект типа Wawe который хранит информацию о волне.
            }
        }

        void SpaunEnemy(string UnitName, int UnitNumber)
        {
            for (int i = 0; i < UnitNumber; i++)
            {
                GameObject Enemy;
                switch (UnitName)
                {
                    case "EnemyLOW":
                        Enemy = GetEnemy(UnitName, "EnemyLOW");
                        map.ContainerSetMaxSide(Enemy.ContainerName, 72);//
                        Enemy.Speed = 1;//
                        Enemy.CanClash = true;
                        Shoot Shoot = new Shoot("shoot", Enemy, Allies);
                        Enemy.Actions.Add(Shoot);
                        Enemy.Recharger = new SimpleRechargen();//
                        Enemy.Recharger.ChargeSpeed = 1;//
                        Enemy.Recharger.ChargeReady = 300;//
                        Enemy.SetHp(72);//
                        Enemy.MaxAmmo = 10;
                        Enemy.AddAmmo(10);
                        Enemy.Range = 150;//
                        break;

                    case "EnemyBoss":
                        Enemy = GetEnemy(UnitName, "platformRed4", "towerRed");
                        map.ContainerSetMaxSide(Enemy.ContainerName, 120);//
                        map.ContainerSetMaxSide(Enemy.Children[0].ContainerName, 100);
                        Enemy.Speed = 7;//
                        Enemy.MaxAmmo = 32;
                        Enemy.AddAmmo(32);
                        Enemy.CanClash = true;
                        Enemy.Recharger = new SimpleRechargen();//
                        Enemy.Recharger.ChargeSpeed = 1;//
                        Enemy.Recharger.ChargeReady = 700;//
                        DefendBase defensBase = new DefendBase("DefBase", Enemy, BaseEnemy);
                        Enemy.Actions.Add(defensBase);
                        Enemy.SetHp(3200);//
                        Enemy.Range = 150;//
                        break;

                }
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

        //TODO: реальизовать деасириалезацию json File

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
            if (CursorMode == 1)
            {
                UnitClick(x, y);
            }
            else if(CursorMode == 2)
            {
                BuildingClick(x, y);
            }
            else if(CursorMode == 3)
            {

            }
        }

        void CheckKey(Key k)
        {
            if (k ==  Key.A && Artillery.Count > 0)
            {
                if(CursorMode == 1)
                {
                    CursorMode = 3;
                    map.Canvas.Cursor = Cursors.Cross;
                }
                else
                {
                    CursorMode = 1;
                    map.Canvas.Cursor = Cursors.Arrow;
                }
            }
        }

        void BuildingClick(int x, int y)
        {
            GameObject Building = null;
            switch (SelectedUnitName) {

                case AllyUnits.БОЕЗАПАС:
                    Building = new GameObject("warehouse", "Building" + counter, SelectedUnitName.ToString());
                    counter++;
                    Allies.Add(Building);
                    AmmoDeports.Add(Building);
                    Building.CanClash = true;
                    CursorMode = 1;
                    PlaceHolder.Visibility = Visibility.Collapsed;
                    break;

                case AllyUnits.МИНА:
                    Building = new GameObject("mine", "Building" + counter, SelectedUnitName.ToString());
                    counter++;
                    //Allies.Add(Building);
                    PlaceHolder.Visibility = Visibility.Collapsed;
                    Mins.Add(Building);
                    CursorMode = 1;
                    break;

                case AllyUnits.Арта:
                    Building = new GameObject("Artillery", "Building" + counter, SelectedUnitName.ToString());
                    counter++;
                    //Allies.Add(Building); Не точно...
                    PlaceHolder.Visibility = Visibility.Collapsed;
                    Artillery.Add(Building);
                    CursorMode = 1;
                    break;
            }
            if (Building != null)
            {
                map.ContainerSetSize(Building.ContainerName, (int)PlaceHolder.Width, (int)PlaceHolder.Height);
                Building.SetCoordinate(x + PlaceHolder.Width / 2, y + PlaceHolder.Height / 2);
                map.ContainerSetAngle(Building.ContainerName, PlaceHolderAngle);
            }
        }

        void UnitClick(int x, int y)
        {
            if (SelectedUnit == null) return;
            if (SelectedUnit.HP == 0) return;
            if (ClickCount > 0)
            {
                SelectedUnit.NeedToRotate = true;
                if (map.Keyboard.IsKeyPressed(Key.Space))
                {
                    //SelectedUnit.NeedToMove = true;
                    SelectedUnit.RemoveAction("move");
                    MoveToTarget move = new MoveToTarget("move", SelectedUnit, SelectedEnemyUnit);
                    SelectedUnit.Actions.Add(move);
                    Rotate rotate = new Rotate("rotate", SelectedUnit, SelectedEnemyUnit);
                    SelectedUnit.RemoveAction("rotate");
                    SelectedUnit.Actions.Add(rotate);
                }
                SelectedUnit.TargetObject = SelectedEnemyUnit;
            }

            else
            {
                SelectedUnit.TargetObject = new GameObject();
                SelectedUnit.TargetObject.SetCoordinate(x, y);
                //SelectedUnit.NeedToMove = true;
                SelectedUnit.NeedToRotate = true;
                //SelectedUnit.Speed = 1;
                SelectedUnit.RemoveAction("move");
                MoveToTarget move = new MoveToTarget("move", SelectedUnit, SelectedUnit.TargetObject);
                SelectedUnit.Actions.Add(move);
                Rotate rotate = new Rotate("rotate", SelectedUnit, SelectedUnit.TargetObject);
                SelectedUnit.RemoveAction("rotate");
                SelectedUnit.Actions.Add(rotate);
            }
        }

        void Build(string UnitName)
        {
            AllyUnits Unit = (AllyUnits)Enum.Parse(typeof(AllyUnits), UnitName, true);
            if (Unit == AllyUnits.ЧИСТОТАНК ||
              Unit == AllyUnits.ПУЛЕМЁТНИК ||
              Unit == AllyUnits.Cборщик_Ресурсов)
            {
                BuildUnit(Unit);
            }

            else
            {
                BuildHouse(Unit);
            }
        }

        void SetPlaceHolderAngle(int angle)
        {
            PlaceHolderAngle = angle;
            RotateTransform rotate = new RotateTransform(PlaceHolderAngle, PlaceHolder.Width / 2, PlaceHolder.Height / 2);
            PlaceHolder.RenderTransform = rotate;
        }

        public AllyUnits SelectedUnitName;

        void BuildHouse(AllyUnits UnitName)
        {
            if (CursorMode == 2) return;
            bool IsBuilt = false;
            switch (UnitName)
            {
                case AllyUnits.БОЕЗАПАС:
                    if (money >= 500)
                    {
                        PlaceHolder.Width = 120;
                        PlaceHolder.Height = 78;
                        AddMoney(-500);
                        IsBuilt = true;
                    }

                    else
                    {
                        CursorMode = 1;
                    }
                    break;

                case AllyUnits.Арта:
                    if (money >= 200)
                    {
                        PlaceHolder.Width = 70;
                        PlaceHolder.Height = 80;
                        AddMoney(-200);
                        IsBuilt = true;
                    }

                    else
                    {
                        CursorMode = 1;
                    }
                    break;
                // Нужно позже разобратся...
                case AllyUnits.МИНА:
                    if (money >= 100)
                    {
                        PlaceHolder.Width = 50;
                        PlaceHolder.Height = 50;
                        AddMoney(-100);
                        IsBuilt = true;
                    }

                    else
                    {
                        CursorMode = 1;
                    }

                    break;
            }
            if (IsBuilt)
            {
                SelectedUnitName = UnitName;
                SetPlaceHolderAngle(0);
                CursorMode = 2;
                PlaceHolder.Visibility = Visibility.Visible;
            }
            
        }


        void BuildUnit(AllyUnits UnitName)
        {
            if (!CanSpawn || basa.HP == 0) return;
            counter++;
            switch (UnitName)
            {
                case AllyUnits.ЧИСТОТАНК:
                    if (money >= 50)
                    {
                        AddMoney(-50);
                        GameObject Tank = new GameObject("Tank_Low_AllY", "Tank_Low_ally" + counter.ToString(), "Tank_Low_ally", CenterX, CenterY, 70);
                        CreatedAnimation.Add(Tank);
                        Allies.Add(Tank);
                        Tank.MaxAmmo = 12;
                        Tank.AddAmmo(12);
                        Tank.Recharger = new SimpleRechargen();
                        Tank.Recharger.ChargeReady = 5000;
                        Tank.Recharger.ChargeSpeed = 10;
                        Tank.SetHp(100);
                        Shoot Shoot = new Shoot("shoot", Tank, Enemis);
                        Tank.Actions.Add(Shoot);
                        Tank.Speed = 1;
                        Tank.SubdivisionNumber = 0;
                        Tank.CanClash = true;
                        //Tank.ContainerWidth = 
                        //Tank.Container = 
                        /*map.ContainerSetFrame(Tank.ContainerName + "Anime", "Fire Bolt1");
                        map.ContainerSetMaxSide(Tank.ContainerName + "Anime", 100);
                        map.AnimationStart(Tank.ContainerName + "Anime", "Fire1", -1);*/
                        obstacle.Add(Tank);
                        Tank.destroyedImage = "Destroyed_Tank_Low_ALLY";
                    }
                    break;
                case AllyUnits.ПУЛЕМЁТНИК:
                    if (money >= 100)
                    {
                        AddMoney(-100);
                        GameObject Tank = new GameObject("ПУЛЕМЁТНИК", "TankMashingan_Medium_ALLY" + counter.ToString(), "TankMashingan_Medium_ALLY", CenterX, CenterY, 55);
                        CreatedAnimation.Add(Tank);
                        Tank.SetHp(120);
                        Tank.MaxAmmo = 1000;
                        Tank.AddAmmo(300);
                        Tank.CanClash = true;
                        Tank.Recharger = new BurstRecharger(30);
                        Tank.Recharger.ChargeReady = 2000;
                        Tank.Recharger.ChargeSpeed = 10;
                        Allies.Add(Tank);
                        Tank.Speed = 2;
                        Shoot Shoot = new Shoot("shoot", Tank, Enemis);
                        Tank.Actions.Add(Shoot);
                        Tank.SubdivisionNumber = 0;
                        obstacle.Add(Tank);
                        Tank.destroyedImage = "Destroyed_Tank_Low_ALLY";
                    }
                    break;

                case AllyUnits.Cборщик_Ресурсов:
                    if (money >= 25)
                    {
                        AddMoney(-25);
                        GameObject Tank = new GameObject("scavenger", "scavenger" + counter.ToString(), "scavenger", CenterX, CenterY, 70);
                        CreatedAnimation.Add(Tank);
                        Allies.Add(Tank);
                        //Tank.Recharger = new SimpleRechargen();
                        //Tank.Recharger.ChargeSpeed = 32;
                        Tank.ContainerWidth = 50;
                        Tank.ContainerHeight = 60;
                        Tank.SetHp(30);
                        Tank.Speed = 3;
                        CollectCrystals crystals = new CollectCrystals("Collector", Tank);
                        Tank.Actions.Add(crystals);
                        Tank.CanClash = true;
                        Tank.SubdivisionNumber = 0;
                        /*map.ContainerSetFrame(Tank.ContainerName + "Anime", "Fire Bolt1");
                        map.ContainerSetMaxSide(Tank.ContainerName + "Anime", 100);
                        map.AnimationStart(Tank.ContainerName + "Anime", "Fire1", -1);*/
                        obstacle.Add(Tank);
                        Tank.destroyedImage = "Destroyed_Tank_Low_ALLY";
                    }
                    break;

                    /*case ""
                        if (money >= 100)
                        {

                        }
                        break;*/
            }
        }

        void ArtilleryShoot()
        {
            if(ArtylleryState == 0)
            {
                ChargeArtilleryReload++;
                if (ChargeArtilleryReload >= ArtilleryLoad)
                    ArtylleryState = 1;
            }
            else if(ArtylleryState == 1)
            {
                
            }
            else
            {

            }
        }

        static public void AddMoney(int NewMoney)
        {
            money = money + NewMoney;
            //ipan.SetText("money", money.ToString());
            tbShowMoney.Text = "Деньги(мифриил)= : " + money;
        }

        void AnimatCreation()
        {
            for (int i = 0; i < CreatedAnimation.Count; i++)
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

        /*void CheckCollision()
        {

        }*/
        void ChekSpawn()
        {
            for (int i = 0; i < Allies.Count; i++)
            {
                if (Allies[i].X < CenterX &&
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

        void CheckMine()
        {
            for(int i = 0; i < Mins.Count; i++)
            {
                for(int j = 0; j < Enemis.Count; j++)
                {
                    if (map.CollisionContainers(Mins[i].ContainerName, Enemis[j].ContainerName))
                    {
                        Enemis[j].AddHp(-50);
                        map.AnimationStart(Mins[i].ContainerName, "Explosion_Collision", 1);
                        Mins.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        void AlliesActions()
        {
            for (int i = 0; i < Allies.Count; i++)
            {
                //Allies[i].Rotate();
                for(int j = 0; j < Allies[i].Actions.Count; j++)
                {
                    Allies[i].Actions[j].Act();
                }
                if(Allies[i].IsDeleted)
                {
                    Allies.RemoveAt(i);
                    i--;
                    continue;
                }

                //Allies[i].PerformAction();
                Allies[i].ReCharge();
                for(int j = 0; j < Allies[i].Children.Count; j++)
                {
                    for(int k = 0; k < Allies[i].Children[j].Actions.Count; k++)
                    {
                        Allies[i].Children[j].Actions[k].Act();
                    }
                }
            }
        }

        void EnemisActions()
        {
            for (int i = 0; i < Enemis.Count; i++)
            {
                //Enemis[i].Move();
                //Enemis[i].Rotate();
                for (int j = 0; j < Enemis[i].Actions.Count; j++)
                {
                    Enemis[i].Actions[j].Act();
                }
                if (Enemis[i].IsDeleted)
                {
                    Enemis.RemoveAt(i);
                    i--;
                    continue;
                }
                Enemis[i].CheckMaxCounter();
                //Enemis[i].PerformAction();
                Enemis[i].ReCharge();
                for (int j = 0; j < Enemis[i].Children.Count; j++)
                {
                    for (int k = 0; k < Enemis[i].Children[j].Actions.Count; k++)
                    {
                        Enemis[i].Children[j].Actions[k].Act();
                    }
                }
            }
        } 

        void AlliesCheckShots()
        {
            for (int i = 0; i < AlliesShots.Count; i++)
            {
                AlliesShots[i].Move();
                for (int j = 0; j < Enemis.Count; j++)
                {
                    if (map.CollisionContainers(AlliesShots[i].ContainerName, Enemis[j].ContainerName))
                    {
                        Enemis[j].AddHp(-AlliesShots[i].GetCharact("damage"));
                        map.AnimationStart(AlliesShots[i].ContainerName, "Explosion_Collision", 1, AlliesShots[i].removeContainer);
                        AlliesShots.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        void EnemisCheckShots()
        {
            for (int i = 0; i < EnemisShots.Count; i++)
            {
                EnemisShots[i].Move();
                for (int j = 0; j < Allies.Count; j++)
                {
                    if (map.CollisionContainers(EnemisShots[i].ContainerName, Allies[j].ContainerName))
                    {
                        Allies[j].AddHp(-EnemisShots[i].GetCharact("damage"));
                        map.AnimationStart(EnemisShots[i].ContainerName, "Explosion_Collision", 1, EnemisShots[i].removeContainer);
                        EnemisShots.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        void CheckCursor()
        {
            Coordinate cord = map.Mouse.GetCursorPosition();
            Canvas.SetLeft(PlaceHolder, cord.X);
            Canvas.SetTop(PlaceHolder, cord.Y);
        }

        void ReloadingAmmo()
        {
            for(int i = 0; i < Allies.Count; i++)
            {
                if (Allies[i].MaxAmmo == 0) continue;
                
                for(int j = 0; j < AmmoDeports.Count; j++)
                {
                    if(map.CollisionContainers(Allies[i].ContainerName, AmmoDeports[j].ContainerName))
                    {
                        Allies[i].AddAmmo(1);
                    }
                }
            }
        }


        void GameCycle()
        {
            CheckMine();
            ReloadingAmmo();
            AnimatCreation();
            ChekSpawn();
            AlliesActions();
            EnemisActions();
            CheckEnemyBase();
            AlliesCheckShots();
            ArtilleryShoot();
            EnemisCheckShots();
            CheckCursor();
            ClickCount--;
        }
    }
}

