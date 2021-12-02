using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defenc
{

    class MoveToTarget: IAction
    {
        GameObject Target; // Обьект к которому движемся
        GameObject Unit; // обьект который движется
        public string Name { get; }
        bool NeedToMove = true;
        public void Act()
        {
            if(NeedToMove)
            {

                Unit.SetCoordinate(Unit.X + Unit.SpeedX, Unit.Y + Unit.SpeedY);
                if (Target != null && Math.Abs(Unit.X - Target.X) < 5 && Math.Abs(Unit.Y - Target.Y) < 5)
                {
                    Unit.RemoveAction("move");
                    NeedToMove = false;
                }
            }
        }
        public MoveToTarget(string name, GameObject unit, GameObject target)
        {
            Name = name;
            Unit = unit;
            Target = target;
        }
    }
        
    }

