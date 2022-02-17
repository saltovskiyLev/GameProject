using System.Collections.Generic;

namespace Tower_Defenc
{
    class Shoot : IAction
    {
        GameObject Unit; // обьект который снаряд :)
        public string Name { get; }
        static int counter;
        public Shoot(string name, GameObject unit, List<GameObject> targets)
        {
            Name = name;
            Unit = unit;
        }
        public void Act()
        {
            if (Unit.CheckAim() && Unit.Recharger != null && Unit.Recharger.CheckCharge() && Unit.HP > 0)
            {
                // выполнение выстрела.                                 
                // Мы дожны созать переменную типа GameObject.           
                // Координаты и угол как у игрокау этого GameObject(а). 
                // Картинка снаряда взовисемонсти от того кто стрелял.   
                // Добавить переменную к списку снарядов.                 
                GameObject Shell = new GameObject("BulletAllies", "Bullet" + counter.ToString(), "BulletAllies");
                counter++;
                if (Unit.SubdivisionNumber == 0)
                {
                    MainWindow.AlliesShots.Add(Shell);
                }
                else
                {
                    MainWindow.EnemisShots.Add(Shell);
                    Unit.GetTarget();
                }
                Shell.Type = "Shell";
                Shell.Speed = 10;
                Shell.SubdivisionNumber = Unit.SubdivisionNumber;
                Shell.SetCoordinate(Unit.X, Unit.Y);
                Shell.NeedToMove = true;
                Shell.SetAngle(Unit.Angle);
                Shell.SetCharacts("damage", 30);
                // задаём размер контейнера.
                MainWindow.map.ContainerSetSize(Shell.ContainerName, 12);
                Unit.Recharger.Reset();
            }
        }
    }
}
