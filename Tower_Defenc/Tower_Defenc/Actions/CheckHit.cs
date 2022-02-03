using System.Collections.Generic;

namespace Tower_Defenc
{
    class CheckHit : IAction
    {
        List<GameObject> Targets;
        GameObject Unit; // обьект который снаряд :)
        public string Name { get; }
        public CheckHit(string name, GameObject unit, List<GameObject> targets)
        {
            Targets = targets;
            Name = name;
            Unit = unit;
        }
        public void Act()
        {
            for (int j = 0; j < Targets.Count; j++)
            {
                if (MainWindow.map.CollisionContainers(Unit.ContainerName, Targets[j].ContainerName))
                {
                    Targets[j].AddHp(-Unit.GetCharact("damage"));
                    MainWindow.map.AnimationStart(Unit.ContainerName, "Explosion_Collision", 1, Unit.removeContainer);
                    Unit.IsDeleted = true;
                    break;
                }
            }
        }
    }
}
