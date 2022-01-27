using System;

namespace Tower_Defenc
{
    class DefendBase : IAction
    {
        public string Name { get; }
        GameObject Target;
        GameObject Unit;
        int Mode = 1;
        double TargetAngle;

        public void Act()
        {
            //throw new NotImplementedException();
            if(Mode == 1)
            {
                if(((Unit.X - Target.X) * (Unit.X - Target.X) + (Unit.Y - Target.Y) * (Unit.Y - Target.Y)) <= 250000)
                {
                    Unit.RemoveAction("move");
                    Unit.RemoveAction("rotate");
                    Mode = 2;
                    RotateToAngle Rotate = new RotateToAngle("RotateToAngle", Unit, Unit.Angle - 90);
                    Unit.Actions.Add(Rotate);
                    TargetAngle = Unit.Angle - 90;
                }
            }

            else if(Mode == 2)
            {
                if(Math.Abs(TargetAngle - Unit.Angle) < 5)
                {
                    Unit.RemoveAction("RotateToAngle");
                    Mode = 3;
                    MoveForward forward = new MoveForward("forward", Unit);
                    BasicRotate basicRotate = new BasicRotate("basicrotate", Unit, 1);
                    Unit.Actions.Add(forward);
                    Unit.Actions.Add(basicRotate);
                    GetTarget target = new GetTarget("target", Unit.Children[0], MainWindow.Allies, false);
                    Unit.Children[0].Actions.Add(target);
                    Unit.Speed = 3;
                    LaunchMissile Raketa = new LaunchMissile("raketa", Unit.Children[0]);
                    Unit.Children[0].Actions.Add(Raketa);
                }
            }
        }

        public DefendBase(string name, GameObject unit, GameObject target)
        {
            Name = name;
            Unit = unit;
            Target = target;

            unit.RemoveAction("move");
            MoveToTarget move = new MoveToTarget("move", unit, target);
            unit.Actions.Add(move);

            unit.RemoveAction("rotate");
            Rotate rotate = new Rotate("rotate", unit, target);
            unit.Actions.Add(rotate);
        }
        // 23451                               
        // 42587                               
        // 12455                                
    }
}



