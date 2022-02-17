namespace Tower_Defenc
{
    class CollectCrystals : IAction
    {
        GameObject Unit; // обьект который собирает
        public string Name { get; } // Имя действия? )))
        public CollectCrystals(string name, GameObject unit)
        {
            Name = name;
            Unit = unit;
        }
        public void Act()
        {
            for(int i = 0; i < MainWindow.Crystals.Count; i++)
            {
                if (MainWindow.map.CollisionContainers(Unit.ContainerName, MainWindow.Crystals[i].ContainerName))
                {
                    MainWindow.map.ContainerErase(MainWindow.Crystals[i].ContainerName);
                    MainWindow.AddMoney(50);
                    MainWindow.Crystals.RemoveAt(i);    
                }
            }
        }
    }
}
