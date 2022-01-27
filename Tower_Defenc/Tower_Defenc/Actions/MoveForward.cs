namespace Tower_Defenc
{
    class MoveForward : IAction
    {
        GameObject Unit; // обьект который поворачивается
        public string Name { get; }
        public MoveForward(string name, GameObject unit)
        {
            Name = name;
            Unit = unit;
        }
        public void Act()
        {
                Unit.SetCoordinate(Unit.X + Unit.SpeedX, Unit.Y + Unit.SpeedY);
        }
    }
}
