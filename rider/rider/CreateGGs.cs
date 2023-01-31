using GameMaps;

namespace rider
{
    class CreateGGs
    {
        public CreateGGs(string Image, int Size, int X_position, int Y_position, string name)
        {
            MainWindow.map.Library.AddContainer(name, name, ContainerType.AutosizedSingleImage);
            // Проблема с ContainerType.AutosizedSingleImage
            MainWindow.map.ContainerSetCoordinate(name, X_position, Y_position);
            MainWindow.map.ContainerSetSize(name, Size);
        }

        public void GGTricks(string nameContainer)
        {
            for (int i = 0; i < 100; i++)
            {
                MainWindow.map.ContainerSetSize(nameContainer, i);
            }
        }
    }
}