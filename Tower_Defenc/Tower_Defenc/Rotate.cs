using GameMaps;

namespace Tower_Defenc
{
    class Rotate : IAction
    {
        GameObject Target; // Обьект к которому движемся
        GameObject Unit; // обьект который поворачивается
        public string Name { get; }
        public Rotate(string name, GameObject unit, GameObject target)
        {
            Name = name;
            Unit = unit;
            Target = target;
        }
        public void Act()
        {
            double TargetAngle = GameMath.GetAngleOfVector(Target.X - Unit.X, Target.Y - Unit.Y);
            if (TargetAngle > Unit.Angle)
            {
                Unit.SetAngle(2, false);
            }
            else
            {
                Unit.SetAngle(-2, false);
            }
        }
    }
        
    }

