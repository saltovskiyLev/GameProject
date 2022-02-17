namespace Tower_Defenc
{
    class CheckFuel : IAction
    {
        GameObject Unit;
        int Fuel; 
        public string Name { get; }
        public CheckFuel(string name, GameObject unit, int fuel)
        {
            Name = name;
            Unit = unit;
            Fuel = fuel;
        }
        public void Act()
        {
            Fuel--;
            if (Fuel == 0)
            {
                MainWindow.map.ContainerSetFrame(Unit.ContainerName, "exp10");
                MainWindow.map.ContainerSetMaxSide(Unit.ContainerName, 20);
                MainWindow.map.AnimationStart(Unit.ContainerName, "Explosion_Collision", 1, Unit.removeContainer);
                Unit.IsDeleted = true;
            }
        }
    }
}
