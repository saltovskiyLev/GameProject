using GameMaps;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tower_Defenc
{
    public class GameObject
    {
        static public UniversalMap_Wpf map;
        public double X;
        public double Y;
        public int HP;
        public bool IsMarked = false;       
        double SpeedX;
        double SpeedY;
        Rectangle HPLINE = new Rectangle();
        public string Type;
        public bool NeedToMove = false;
        public int TargetX;
        public int TargetY;
        public int Angle;
        public double Speed;
        public string ContainerName;
        /// <summary>
        /// Пошли функции
        /// </summary>
        public GameObject(string PICTURE, string Container, string type)
        {
            map.Library.AddContainer(Container, PICTURE, ContainerType.AutosizedSingleImage);
            ContainerName = Container;
            map.ContainerSetLeftClickHandler(ContainerName, ClickType.Left, LeftClick);
            map.Library.AddContainer(ContainerName + "mark", "AllyScope", ContainerType.AutosizedSingleImage);
            Type = type;
            HPLINE.Height = 0;
            HPLINE.Fill = Brushes.Green;
            map.Canvas.Children.Add(HPLINE);
            //map.DrawRectangle((int)X - 35, (int)Y + 25, hp, 10, Brushes.Green, Brushes.Green);
        }
        public GameObject(string PICTURE, string Container, string type, int x, int y, int size): this(PICTURE, Container, type)
        {
            map.ContainerSetMaxSide(ContainerName, 70);
            SetCoordinate(x, y);
            map.ContainerSetAngle(ContainerName, 180);
            map.ContainerSetZIndex(ContainerName, 87);

        }

        public void SetCoordinate(double x, double y)
        {
            X = x;
            Y = y;
            Canvas.SetTop(HPLINE, Y + 35);
            Canvas.SetLeft(HPLINE, X - 35);
            map.ContainerSetCoordinate(ContainerName, x, y);
            if(IsMarked)
            {
                map.ContainerSetCoordinate(ContainerName + "mark", X, Y);
            }
        }

        public void LeftClick()
        {
            if (Type == "basa") return;
            if(MainWindow.SelectedUnit != null)
            {
                MainWindow.SelectedUnit.IsMarked = false;
                map.ContainerSetCoordinate(MainWindow.SelectedUnit.ContainerName + "mark", -1000, -1000);
                MainWindow.SelectedUnit.HPLINE.Height = 0;
            }
            MainWindow.SelectedUnit = this;
            IsMarked = true;
            map.ContainerSetMaxSide(ContainerName + "mark", 62);
            HPLINE.Height = 10;
        }

        public void Move()
        {
            if(NeedToMove)
            {
                SetCoordinate(X + SpeedX, Y + SpeedY);
                double TargetAngle = GameMath.GetAngleOfVector(TargetX - X, TargetY - Y);
                if (TargetAngle > Angle)
                {
                    SetAngle(Angle + 2);
                }
                else
                {
                    SetAngle(Angle - 2);
                }
                if(Math.Abs(X - TargetX) < 5 && Math.Abs(Y - TargetY) < 5)
                {
                    NeedToMove = false;
                }
            }
        }

        public void SetAngle(int angle) {
            Angle = angle;
            map.ContainerSetAngle(ContainerName, angle);
            SpeedX = Speed * Math.Cos(GameMath.DegreesToRad(angle));
            SpeedY = Speed * Math.Sin(GameMath.DegreesToRad(angle)); 
        }

        public void SetHp(int hp)
        {
            HP = hp;
            HPLINE.Width = HP / 2; 
        }
    }
}