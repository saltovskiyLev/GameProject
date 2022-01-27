namespace Tower_Defenc
{
    class LaunchMissile : IAction
    {
        GameObject Unit; // обьект который поворачивается
        public string Name { get; }
        int r = 0;
        int Count = 0;
        public LaunchMissile(string name, GameObject unit)
        {
            Name = name;
            Unit = unit;
        }
        public void Act()
        {
            Unit.SetCoordinate(Unit.X + Unit.SpeedX, Unit.Y + Unit.SpeedY);
            r += 10;
            Count++;
            if(r >= 2000)
            {
                r = 0;
                GameObject RAKETA = new GameObject("RAKETA", Unit.ContainerName + "R" + Count, "Missile", (int)Unit.X, (int)Unit.Y, 32);

                RAKETA.Speed = 7;

                MoveToTarget move = new MoveToTarget("move", RAKETA, Unit.SelectedObject);
                Unit.Actions.Add(move);

                Tower_Defenc.Rotate rotate = new Rotate("rotate", RAKETA, Unit.SelectedObject);
                Unit.Actions.Add(rotate);
            }
        }
    }
}
