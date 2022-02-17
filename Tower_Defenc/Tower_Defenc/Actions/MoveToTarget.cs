using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defenc
{

    class MoveToTarget : IAction
    {
        GameObject Target; // Обьект к которому движемся
        GameObject Unit; // Oбьект который движется
        public string Name { get; }
        bool NeedToMove = true;
        int Radius;
        public void Act()
        {
            if (NeedToMove)
            {

                Unit.SetCoordinate(Unit.X + Unit.SpeedX, Unit.Y + Unit.SpeedY);
                if (Target != null && Math.Abs(Unit.X - Target.X) < Radius && Math.Abs(Unit.Y - Target.Y) < Radius)
                {
                    Unit.RemoveAction("move");
                    NeedToMove = false;
                }
            }
        }
        public MoveToTarget(string name, GameObject unit, GameObject target, int RADIUS = 5)
        {
            Name = name;
            Unit = unit;
            Target = target;
            Radius = RADIUS;
        }
    }
}

