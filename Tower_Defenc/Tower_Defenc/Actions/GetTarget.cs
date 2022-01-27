using System.Collections.Generic;

namespace Tower_Defenc
{
    class GetTarget : IAction
    {
        GameObject Unit; // обьект который поворачивается
        GameObject TargetObject;
        List<GameObject> Targets;
        bool NeedToMove;
        public string Name { get; }
        public GetTarget(string name, GameObject unit, List<GameObject> targets, bool needToMove)
        {
            Name = name;
            Unit = unit;
            Targets = targets;
            NeedToMove = needToMove;
        }
        public void Act()
        {
            double DistansToTarget; // хранит расстояние до выбранной цели.
            if (TargetObject != null) // если цель уже есть то вычесляем расстояние до цели
            {
                DistansToTarget = (TargetObject.X - Unit.X) * (TargetObject.X - Unit.X) + (TargetObject.Y - Unit.Y) * (TargetObject.Y - Unit.Y);
            }
            else
            {
                DistansToTarget = 100000000; // если цели ещё нет
            }
            for (int i = 0; i < Targets.Count; i++) // перебераем возмодные цели цели 
            {
                double NewDistans = (Targets[i].X - Unit.X) * (Targets[i].X - Unit.X) + (Targets[i].Y - Unit.Y) * (Targets[i].Y - Unit.Y);
                // вычисляем расстояние с еомером i елси оно меньше чем расстояние до выбранной цели то запоминаем новое расстояние и новую цель
                if (NewDistans < DistansToTarget)
                {
                    DistansToTarget = NewDistans;
                    TargetObject = Targets[i];
                    if (NeedToMove)
                    {
                        Unit.RemoveAction("move");
                        MoveToTarget move = new MoveToTarget("move", Unit, Targets[i]);
                        Unit.Actions.Add(move);
                    }
                    Unit.RemoveAction("rotate");
                    Tower_Defenc.Rotate rotate = new Rotate("rotate", Unit, Targets[i]);
                    Unit.Actions.Add(rotate);
                    Unit.SelectedObject = TargetObject;
                }
            }
        }
    }
}
