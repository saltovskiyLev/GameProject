namespace Tower_Defenc
{
    class BasicRotate : IAction
    {
        GameObject Unit; // обьект который поворачивается
        public string Name { get; }
        public int RotationSpeed;
        public BasicRotate(string name, GameObject unit, int rotationspeed)
        {
            Name = name;
            Unit = unit;
            RotationSpeed = rotationspeed;
        }
        public void Act()
        {
            Unit.SetAngle(RotationSpeed, false);
        }
    }
}
