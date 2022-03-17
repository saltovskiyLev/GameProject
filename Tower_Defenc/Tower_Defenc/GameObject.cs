using GameMaps;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Collections.Generic;

namespace Tower_Defenc
{
    public class GameObject
    {
        static public UniversalMap_Wpf map;
        // ПОДСКАЗАКА ПОДСКАЗАКА ПОДСКАЗАКА ПОДСКАЗАКА ПОДСКАЗАКА ПОДСКАЗАКА ПОДСКАЗАКА
        // если state = 0, то с танком все ОК.
        // если state = 1, то танк ДЫМИТСЯ.
        // если state = 2, то танк ГОРИТ.
        // есди state = 3, то танк УНИЧТОЖЕН.
        // Ну можно ещё что то дописать.


        // если SubdivisionNumber = 0, НАШ.
        // если SubdivisionNumber = 1, ВРАГ самаходка LOW.
        Int64 state = 0;   
        int MaxCounter;
        //int ChargeRate = 5000000;
        //public int ChargeReady;
        //public int ChargeSpeed;
        public IRecharger Recharger;
        static int counter = 0;
        //public int TowerAngle;
        public bool NeedToRotate = false;
        public int SubdivisionNumber;
        public List<GameObject> Children = new List<GameObject>(); 
        public int TargetRadiusEnemy = 700;
        //public string TowerContainerName = "";
        public int Range = 5;
        int DamageCounter = 2000;
        public bool CanClash = false;  
        public double X { get; private set; }
        public double Y { get; private set; }
        public List<GameObject> targets = new List<GameObject>(); // список целей для атаки
        public int HP { get; private set; }
        public List<IAction> Actions = new List<IAction>();
        public bool IsMarked = false;
        public int Ammo;
        public int MaxAmmo;
        public string destroyedImage;
        public bool IsDeleted = false;
        public string destroyedAmimathion = "";
        public double SpeedX { get; private set; }
        public double SpeedY { get; private set; }
        Dictionary<string, int> characts = new Dictionary<string, int>();
        public int mode; // 0 - Обьект стоит на месте   // 1 - Обьект движется в точку на карте // 2 - Обьект движется к другому игровому обьекту;
        public GameObject TargetObject;
        public GameObject SelectedObject;
        Rectangle HPLINE = new Rectangle();
        Rectangle AMMOLINE = new Rectangle();
        public bool NeedToMove = false;
        bool CanSetCharacts = true;
        //public int TargetX;
        //public int TargetY; 
        public int Angle;
        public double Speed;
        public string Type;
        public string ContainerName;
        /// <summary>
        /// Пошли функции
        /// </summary>
        public GameObject(string PICTURE, string Container, string type)
        {
            map.Library.AddContainer(Container, PICTURE, ContainerType.AutosizedSingleImage);
            ContainerName = Container;
            map.ContainerSetLeftClickHandler(ContainerName, ClickType.Left, LeftClick);
            map.Library.AddContainer(ContainerName + "top", "nothing", ContainerType.AutosizedSingleImage);
            map.Library.AddContainer(ContainerName + "mark", "AllyScope", ContainerType.AutosizedSingleImage);
            map.Library.AddContainer(ContainerName + "Anime", "nothing", ContainerType.AutosizedSingleImage);
            Type = type;

            HPLINE.Height = 10;
            HPLINE.Fill = Brushes.Green;
            map.Canvas.Children.Add(HPLINE);

            AMMOLINE.Height = 10;
            AMMOLINE.Fill = Brushes.Yellow;
            map.Canvas.Children.Add(AMMOLINE);

            map.ContainerSetZIndex(ContainerName + "Anime", 86);
            //map.DrawRectangle((int)X - 35, (int)Y + 25, hp, 10, Brushes.Green, Brushes.Green);
        }

        public GameObject(string PICTURE, string TowerPicture, string Container, string type): this(PICTURE, Container, type)
        {
            //map.ContainerSetMaxSide(ContainerName + "_2", )
            GameObject Tower = new GameObject(TowerPicture, ContainerName + "_2", "TOWER");
            map.ContainerSetZIndex(Tower.ContainerName, 120);
            Children.Add(Tower);

        }


        public GameObject()
        {
            ////
        }

        public int GetCharact(string key)
        {
            int result = 0;
            if(characts.ContainsKey(key))
            {
                result = characts[key];
            }
            return result;
        }

        public void GetTarget()
        {
            double DistansToTarget; // хранит расстояние до выбранной цели.
            if (TargetObject != null) // если цель уже есть то вычесляем расстояние до цели
            {
                DistansToTarget = (TargetObject.X - X) * (TargetObject.X - X) + (TargetObject.Y - Y) * (TargetObject.Y - Y);
            }
            else
            {
                DistansToTarget = 100000000; // если цели ещё нет
            }
            for (int i = 0; i < targets.Count; i++) // перебераем возмодные цели цели 
            {
                double NewDistans = (targets[i].X - X) * (targets[i].X - X) + (targets[i].Y - Y) * (targets[i].Y - Y); 
                // вычисляем расстояние с еомером i елси оно меньше чем расстояние до выбранной цели то запоминаем новое расстояние и новую цель
                if (NewDistans < DistansToTarget) 
                {
                    DistansToTarget = NewDistans;
                    TargetObject = targets[i];

                    RemoveAction("move");
                    MoveToTarget move = new MoveToTarget("move", this, targets[i]);
                    Actions.Add(move);
                    RemoveAction("rotate");
                    Tower_Defenc.Rotate rotate = new Rotate("rotate", this, targets[i]);
                    Actions.Add(rotate);
                }
            }
        }

        public void SetCharacts(string key, int Val)
        {
            if(CanSetCharacts)
            {
                characts.Add(key, Val);
            }
        }

        public void Block()
        {
            CanSetCharacts = false;
        }

        public GameObject(string PICTURE, string Container, string type, int x, int y, int size): this(PICTURE, Container, type)
        {
            map.ContainerSetMaxSide(ContainerName, size);
            SetCoordinate(x, y);
            map.ContainerSetAngle(ContainerName, 180);
            map.ContainerSetZIndex(ContainerName, 87);
        }

        public void CheckObject()
        {
            CheckMaxCounter();
        }

        public void CheckMaxCounter()
        {
            if(MaxCounter >= 2000)  
            {
                MaxCounter = -900000000;
                map.ContainerSetFrame(ContainerName + "mark", "nothing");
            }
            else
            {
                MaxCounter = MaxCounter + 10;
            }
        }

        public void removeContainer()
        {
            map.ContainerErase(ContainerName);
            // Реализация удалениЯ контейнера после анимации взрыва.
        }

        public void SetCoordinate(double x, double y)
        {

            if (string.IsNullOrEmpty(ContainerName))
            {
                X = x;
                Y = y;
            }
            else
            {
                map.ContainerMovePreview(ContainerName, x, y, Angle);
                // Этот цикл должен быть в отдельной функции проверки столкновения обьектов.
                // Та функция вызывается в GameCycle.
                if (Type != "Shell" && CanClash)
                {
                    for (int i = 0; i < MainWindow.obstacle.Count; i++)
                    {
                        if (map.CollisionContainers(ContainerName, MainWindow.obstacle[i].ContainerName, true) &&
                           ContainerName != MainWindow.obstacle[i].ContainerName)
                        {
                            CheckDamageCounter();
                            MainWindow.obstacle[i].CheckDamageCounter();
                            return;
                        }
                    }
                }
                //Debug.WriteLine("Name = {0}, x = {1}, y = {2}", ContainerName, x, y);
                X = x;
                Y = y;

                Canvas.SetTop(HPLINE, Y + 35);
                Canvas.SetLeft(HPLINE, X - 35);
                Canvas.SetTop(AMMOLINE, Y + 55);
                Canvas.SetLeft(AMMOLINE, X - 35);
                map.ContainerSetCoordinate(ContainerName, x, y);

                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].SetCoordinate(x, y);
                    //map.ContainerSetCoordinate(Children[i].ContainerName, x, y);
                }
                if (IsMarked)
                {
                    map.ContainerSetCoordinate(ContainerName + "mark", X, Y);
                }
                map.ContainerSetCoordinate(ContainerName + "Anime", X, Y);
            }
        }

        public void CheckDamageCounter()
        {
            if (DamageCounter >= 2000)
            {
                //MainWindow.Allies[i].SetHp(MainWindow.Allies[i].HP - 1);
                map.ContainerSetFrame(ContainerName + "top", "exp1");
                map.ContainerSetMaxSide(ContainerName + "top", 42);
                map.ContainerSetCoordinate(ContainerName + "top", X, Y);
                map.AnimationStart(ContainerName + "top", "Explosion_Collision", 1, StopAnimation);
                AddHp(-20);
                DamageCounter = 0;
            }
            else
            {
                DamageCounter = DamageCounter + 20;
            }
        }

        void StopAnimation()
        {
            map.ContainerSetFrame(ContainerName + "top", "nothing");
        }

        public void LeftClick()
        {
            if (SubdivisionNumber == 0)
            {
                if (HP == 0) return;
                if (Type == "basa") return;
                if (MainWindow.SelectedUnit != null)
                {
                    MainWindow.SelectedUnit.IsMarked = false;
                    map.ContainerSetCoordinate(MainWindow.SelectedUnit.ContainerName + "mark", -1000, -1000);
                    //MainWindow.SelectedUnit.HPLINE.Height = 0;99
                }
                MainWindow.SelectedUnit = this;
                IsMarked = true;
                map.ContainerSetMaxSide(ContainerName + "mark", 62);
            }
            else
            {
                map.ContainerSetFrame(ContainerName + "mark", "MarkEnemy");
                map.ContainerSetMaxSide(ContainerName + "mark", 70);
                IsMarked = true;
                MaxCounter = 0;
                MainWindow.SelectedEnemyUnit = this;
                MainWindow.ClickCount = 5;
                if(MainWindow.SelectedUnit != null)
                {
                    MainWindow.SelectedUnit.mode = 2;
                }
            }
        }

        public void Move()
        {
            if (NeedToMove)
            {
                SetCoordinate(X + SpeedX, Y + SpeedY);
                if (TargetObject != null && Math.Abs(X - TargetObject.X) < Range && Math.Abs(Y - TargetObject.Y) < Range)
                {
                    NeedToMove = false;
                    NeedToRotate = false;
                }
            }
            // debug.writeline("{0}, {1}", x, y);
        }

        public bool CheckAim()
        {
            if (TargetObject == null) return false;

            bool IsAimed = false;
            double AngleToVector = GameMath.GetAngleOfVector(TargetObject.X - X, TargetObject.Y - Y);
            if(Math.Abs(Angle - AngleToVector) < 3)
            {
                IsAimed = true;
            }

            return IsAimed;
        }

        /*ublic void PerformAction()
         {

            if (mode == 2 && CheckAim() && Recharger != null && Recharger.CheckCharge() && HP > 0)
            {
                // выполнение выстрела.                                 
                // Мы дожны созать переменную типа GameObject.           
                // Координаты и угол как у игрокау этого GameObject(а). 
                // Картинка снаряда взовисемонсти от того кто стрелял.   
                // Добавить переменную к списку снарядов.                 
                GameObject Shell = new GameObject("BulletAllies", "Bullet" + counter.ToString(), "BulletAllies");
                counter++;
                if (SubdivisionNumber == 0)
                {
                    MainWindow.AlliesShots.Add(Shell);
                }
                else
                {
                    MainWindow.EnemisShots.Add(Shell);
                    GetTarget();
                }
                Shell.Type = "Shell";
                Shell.Speed = 10;
                Shell.SubdivisionNumber = SubdivisionNumber;
                Shell.SetCoordinate(X, Y);
                Shell.NeedToMove = true;
                Shell.SetAngle(Angle);
                Shell.SetCharacts("damage", 30);
                // задаём размер контейнера.
                map.ContainerSetSize(Shell.ContainerName, 12);
                Recharger.Reset();
            }
        }*/

        public void ReCharge()
        {
            //ChargeRate += ChargeSpeed;
            if(Recharger != null)
            {
                Recharger.ReCharge();
            }
        }

        public void Rotate()
        {
            if (NeedToRotate)
            {
                double TargetAngle = GameMath.GetAngleOfVector(TargetObject.X - X, TargetObject.Y - Y);
                if (TargetAngle > Angle)
                {
                    SetAngle(2, false);
                }
                else
                {
                    SetAngle(-2, false);
                }
            }
        }

        public void SetAngle(int angle, bool IsAbsolute = true) {
            if(IsAbsolute)
            {
                Angle = angle;
                for(int i = 0; i < Children.Count; i++)
                {
                    Children[i].SetAngle(angle);
                }
            }

            else
            {
                Angle += angle;
                for (int i = 0; i < Children.Count; i++)
                {
                    Children[i].SetAngle(angle, false);
                }
            }
            map.ContainerSetAngle(ContainerName, Angle);
            SpeedX = Speed * Math.Cos(GameMath.DegreesToRad(Angle));
            SpeedY = Speed * Math.Sin(GameMath.DegreesToRad(Angle));
            map.ContainerSetAngle(ContainerName + "Anime", Angle);
        }


        void CheckHp()
        {
            if (HP < 0)
            {
                HP = 0;
            }
            if (state == 0 && HP >= 20 && HP < 50)
            {

            }
            else if (state != 2 && HP < 20 && HP > 0)
            {
                map.ContainerSetFrame(ContainerName + "Anime", "Fire Bolt1");
                map.ContainerSetMaxSide(ContainerName + "Anime", 100);
                map.AnimationStart(ContainerName + "Anime", "Fire1", -1);
                state = 2;
            }
            else if (state != 3 && HP <= 0)
            {
                // Если есть анимация уничтожение то воспроизводим её иначе вызываем SetDestroyedImage
                //map.ContainerSetFrame(ContainerName, "Destroyed_Tank_Low_ALLY");
                if(destroyedAmimathion!= "")
                {
                    map.AnimationStart(ContainerName, destroyedAmimathion, 1, SetDestroyedImage);
                }    
                else
                {
                    SetDestroyedImage();
                }
                map.ContainerSetFrame(ContainerName + "mark", "nothing");
                //NeedToMove = false;
                NeedToRotate = false;
                Actions.Clear();
                map.AnimationStop(ContainerName + "Anime", "Fire1");
                map.ContainerSetFrame(ContainerName + "Anime", "nothing");
            }
            HPLINE.Width = HP / 2;
            
        }

        public void AddAmmo(int ammo)
        {
            Ammo = Ammo + ammo;
            AMMOLINE.Width = (double)Ammo / MaxAmmo * 70;
        }

        void SetDestroyedImage()
        {
            map.ContainerSetFrame(ContainerName, destroyedImage);
        }

        public void SetHp(int hp)
        {
            HP = hp;
            CheckHp();
        }

        public void AddHp(int HpChange)
        {
            HP += HpChange;
            CheckHp();
        }

        public void RemoveAction(string Name)
        {
            for(int i = 0; i < Actions.Count; i++)
            {
                if(Name == Actions[i].Name)
                {
                    Actions.RemoveAt(i);
                    return;
                }
            } 
        }
    }
}
