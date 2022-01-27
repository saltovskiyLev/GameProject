namespace Tower_Defenc
{
    class RotateToAngle : IAction
    {
        GameObject Unit; // Oбьект который движется
        double Angle;
        public string Name { get; }
        public void Act()
        {
            if(Angle - Unit.Angle > 3)
            {
                Unit.SetAngle(1 , false);
            }

            if (Angle - Unit.Angle < -3)
            {
                Unit.SetAngle(-1, false);
            }
        }
        public RotateToAngle(string name, GameObject unit, double angle)
        {
            Name = name;
            Unit = unit;
            Angle = angle;
        }
    }
}

