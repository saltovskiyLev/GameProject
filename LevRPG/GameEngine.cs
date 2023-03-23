using GameMaps;
using System;
using System.Collections.Generic;

namespace LevRPG
{
    class GameEngine
    {
        UniversalMap_Wpf map;
        public List<TerainObject> terainObjects = new List<TerainObject>();


        public GameEngine(UniversalMap_Wpf _map)
        {
            map = _map;
        }
        
        public void AddTerrainObject(TerainObject obj)
        {
            map.Library.AddContainer(obj.Id, obj.Image, ContainerType.TiledImage);
            
            map.ContainerSetSize(obj.Id, Math.Abs(obj.x1 - obj.x2), Math.Abs(obj.y1 - obj.y2));
            map.ContainerSetTileSize(obj.Id, 40, 40);
            map.ContainerSetCoordinate(obj.Id, (obj.x1 + obj.x2) / 2, (obj.y1 + obj.y2) / 2);
         terainObjects.Add(obj);
        }

        public void AddNPCObject(NPC npc)
        {
            map.Library.AddContainer(npc.id, npc.Image, ContainerType.AutosizedSingleImage);
            map.ContainerSetCoordinate(npc.id, npc.CenterX, npc.CenterY);
            map.ContainerSetSize(npc.id, npc.MaxSide);
            map.ContainerSetAngle(npc.id, npc.Angle);
        }

        public void Move(NPC npc)
        {
            map.ContainerSetCoordinate(npc.id, npc.CenterX, npc.CenterY);
            map.ContainerSetAngle(npc.id, npc.Angle);
        }

        public void ReadMap(MapData map)
        {
            for(int i = 0; i < map.terainObjects.Count; i++)
            {
                AddTerrainObject(map.terainObjects[i]);
                
            }
        }

        public bool CanMove(NPC npc, double dX, double dY, int dAngle)
        {
            bool CanMove = true;
            map.ContainerMovePreview(npc.id, npc.CenterX + dX, npc.CenterY + dY, npc.Angle + dAngle);

            for(int i = 0; i < terainObjects.Count; i++)
            {
                if (map.CollisionContainers(npc.id, terainObjects[i].Id, true))
                {
                    CanMove = false;
                    break;
                }    
            }

            return CanMove;


        }
    }
}
