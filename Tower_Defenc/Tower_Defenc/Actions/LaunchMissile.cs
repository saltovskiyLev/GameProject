using System.Collections.Generic;

namespace Tower_Defenc
{
    class LaunchMissile : IAction
    {
        GameObject Unit; // Объект который стреляет;
        public string Name { get; }
        int r = 0;
        List<GameObject> Targets;
        int Count = 0;
        public LaunchMissile(string name, GameObject unit, List<GameObject> targets)
        {
            Name = name;
            Targets = targets;
            Unit = unit;
        }
        public void Act()
        {
            //Unit.SetCoordinate(Unit.X + Unit.SpeedX, Unit.Y + Unit.SpeedY);
            r += 10;
            Count++;
            if(r >= 2000)
            {
                r = 0;
                GameObject RAKETA = new GameObject("RAKETA", Unit.ContainerName + "R" + Count, "Shell", (int)Unit.X, (int)Unit.Y, 32);
                
                RAKETA.Speed = 7;

                MoveToTarget move = new MoveToTarget("move", RAKETA, Unit.SelectedObject);
                RAKETA.Actions.Add(move);

                Tower_Defenc.Rotate rotate = new Rotate("rotate", RAKETA, Unit.SelectedObject);
                RAKETA.Actions.Add(rotate);

                CheckHit CheckHit = new CheckHit("CheckHit", RAKETA, Targets);
                RAKETA.Actions.Add(CheckHit);

                MainWindow.Enemis.Add(RAKETA);
                RAKETA.SetCharacts("damage", 35);
            }
        }
    }
}
