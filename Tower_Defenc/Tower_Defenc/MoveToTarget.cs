﻿using System;
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
        public void Act()
        {
            if (NeedToMove)
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
    class DefensBase : IAction
    {
        public string Name { get; }
        GameObject Target;
        GameObject Unit;

        public void Act()
        {
            //throw new NotImplementedException();
        }

        public DefensBase(string name, GameObject unit, GameObject target)
        {
            Name = name;
            Unit = unit;
            Target = target;
            unit.RemoveAction("move");
            MoveToTarget move = new MoveToTarget("move", unit, target);
            unit.Actions.Add(move);
        }
        // 23451                               
        // 42587                               
        // 12455                                
    }
}

