using GameMaps;

namespace Морской_Бой
{
    class ShipCoordinate : Coordinate
    {
        public bool IsDestroyed = false;

        public ShipCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
