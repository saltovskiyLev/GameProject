using GameMaps;
using System;

namespace Tower_Defenc
{
    public class GameObject
    {
        static public UniversalMap_Wpf map;
        public double X;
        public double Y;
        public bool IsMarked = false;       
        double SpeedX;
        double SpeedY;
        public bool NeedToRotate = false;
        public int TargetX;
        public int TargetY;
        public int Angle;
        public double Speed;
        public string ContainerName;
        /// <summary>
        /// Пошли функции
        /// </summary>
        public GameObject(string PICTURE, string Container)
        {
            map.Library.AddContainer(Container, PICTURE, ContainerType.AutosizedSingleImage);
            ContainerName = Container;
            map.ContainerSetLeftClickHandler(ContainerName, ClickType.Left, LeftClick);
            //map.Library.AddContainer();
        }
        public GameObject(string PICTURE, string Container, int x, int y, int size): this(PICTURE, Container)
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
            map.ContainerSetCoordinate(ContainerName, x, y);
            if(IsMarked)
            {
                //map.ContainerSetCoordinate();
            }
        }

        public void LeftClick()
        {
            MainWindow.SelectedUnit = this;
        }

        public void Move()
        {
            if(NeedToRotate)
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
                    NeedToRotate = false;
                }
            }
        }

        public void SetAngle(int angle) {
            Angle = angle;
            map.ContainerSetAngle(ContainerName, angle);
            SpeedX = Speed * Math.Cos(GameMath.DegreesToRad(angle));
            SpeedY = Speed * Math.Sin(GameMath.DegreesToRad(angle)); 
        }
    }
}