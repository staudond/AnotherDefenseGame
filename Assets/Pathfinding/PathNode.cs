using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public Vector2Int coordinates;

    public int gCost;
    public int hCost;
    public int fCost;
    public bool isRoad;

    public PathNode previousNode;

    public PathNode(int x, int y, MapTile[,] map)
    {
        coordinates = new Vector2Int(x,y);
        isRoad = map[x, y].isRoad;
    }

    public PathNode(Vector2Int coordinates,MapTile[,] map)
    {
        this.coordinates = coordinates;
        isRoad = map[coordinates.x, coordinates.y].isRoad;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
