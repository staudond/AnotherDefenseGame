using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int DIAGONAL_COST = 15;
    private const int STRAIGHT_COST = 10;

    private List<PathNode> openList;
    private List<PathNode> closedList;
    private PathNode[,] grid;
    

    private int width;
    private int height;

    private MapTile[,] map;
    public Pathfinding( MapTile[,] map)
    {
        this.map = map;
        this.width = map.GetLength(0);
        this.height = map.GetLength(1);
        grid = createGrid(width,height);
    }

    private PathNode[,] createGrid(int width, int height)
    {
        PathNode[,] grid = new PathNode[width,height];
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j] = new PathNode(i, j,map);
            }
        }
        return grid;
    }

    public List<Vector2Int> VectorFindPath(List<Vector2Int> starts, Vector2Int end)
    {
        List<PathNode> path = FindPath(starts, end);
        
        if (path == null)
        {
            return null;
        }

        List<Vector2Int> vectorPath = new List<Vector2Int>();
        foreach (PathNode node in path)
        {
            vectorPath.Add(node.coordinates);
        }

        return vectorPath;

    }


    public List<Vector2Int> VectorFindPath(Vector2Int start, List<Vector2Int> ends)
    {
        List<Vector2Int> vectorPath = VectorFindPath(ends, start);
        
        vectorPath.Reverse();
        
        return vectorPath;
    }

    public List<PathNode> FindPath(Vector2Int start, List<Vector2Int>  ends)
    {
        //to use a* for multiple end notes, we need to use them as start nodes,
        //and the start node as the end node and then reverse the result
        List<PathNode> path = FindPath(ends, start);
        path.Reverse();
        return path;
    }
    
    
    public List<PathNode> FindPath(List<Vector2Int> starts, Vector2Int end)
    {
        List<PathNode> startNodes = new List<PathNode>();
        foreach (Vector2Int startCoordinate in starts)
        {
            startNodes.Add(GetNode(startCoordinate));
        }
               
        PathNode endNode = GetNode(end);

        
        bool invalid = true;
        foreach (PathNode node in startNodes)
        {
            if (node != null)
            {
                invalid = false;
                break;
            }
        }

        if (endNode == null || invalid)
        {
            return null;
        }

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                PathNode pathNode = GetNode(i, j);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.previousNode = null;
            }
        }
        
        openList = new List<PathNode>();
        foreach (PathNode node in startNodes)
        {
            node.gCost = 0;
            node.hCost = CalculateDistance(node, endNode);
            node.CalculateFCost();
            openList.Add(node);
        }
        
        closedList = new List<PathNode>();

        while (openList.Count > 0)
        {
            PathNode current = GetLowestFCostNode(openList);
            if (current == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (PathNode neighbour in GetNeighbours(current))
            {
                if (closedList.Contains(neighbour)) continue;

                if (!neighbour.isRoad)
                {
                    closedList.Add(neighbour);
                    continue;
                }

                int tentativeGCost = current.gCost + CalculateDistance(current, neighbour);
                if (tentativeGCost < neighbour.gCost)
                {
                    neighbour.previousNode = current;
                    neighbour.gCost = tentativeGCost;
                    neighbour.hCost = CalculateDistance(neighbour, endNode);
                    neighbour.CalculateFCost();

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }

            }

        }
        //out of nodes on the open list and didn"t reached the end node
        return null;

    }
    
    private List<PathNode> GetNeighbours(PathNode current)
    {
        List<PathNode> neighbours = new List<PathNode>();

        if (current.coordinates.x - 1 >= 0)
        {
            //left
            neighbours.Add(GetNode(current.coordinates.x -1,current.coordinates.y));
            
            //upper left
            if (current.coordinates.y -1 >= 0)
            {
                neighbours.Add(GetNode(current.coordinates.x-1,current.coordinates.y-1));
            }
            //bottom left
            if (current.coordinates.y + 1 < height)
            {
                neighbours.Add(GetNode(current.coordinates.x-1,current.coordinates.y+1));
            }
        }

        if (current.coordinates.x + 1 < width )
        {
            //righ
            neighbours.Add(GetNode(current.coordinates.x +1,current.coordinates.y));
            //upper right
            if (current.coordinates.y -1 >= 0)
            {
                neighbours.Add(GetNode(current.coordinates.x+1,current.coordinates.y-1));
            }
            //bottom right
            if (current.coordinates.y + 1 < height)
            {
                neighbours.Add(GetNode(current.coordinates.x+1,current.coordinates.y+1));
            }
        }

        //up
        if (current.coordinates.y - 1 >= 0)
        {
            neighbours.Add(GetNode(current.coordinates.x,current.coordinates.y - 1));
        }
        
        //down
        if (current.coordinates.y + 1 < height)
        {
            neighbours.Add(GetNode(current.coordinates.x,current.coordinates.y + 1));
        }

        return neighbours;
    }

    public PathNode GetNode(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid.GetLength(0) && y < grid.GetLength(1))
        {
            return grid[x, y];
        }

        return default(PathNode);
    }

    public PathNode GetNode(Vector2Int coordinates)
    {
        if (coordinates.x >= 0 && coordinates.y >= 0 && 
            coordinates.x < grid.GetLength(0) && coordinates.y < grid.GetLength(1))
        {
            return grid[coordinates.x, coordinates.y];
        }

        return default(PathNode);
    }

    private int CalculateDistance(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.coordinates.x - b.coordinates.x);
        int yDistance = Mathf.Abs(a.coordinates.y - b.coordinates.y);
        int rest = Mathf.Abs(xDistance - yDistance);
        return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * rest;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
    {
        PathNode lowestFCostNode = pathNodes[0];
        for (int i = 0; i < pathNodes.Count; i++)
        {
            if (pathNodes[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodes[i];
            }
        }

        return lowestFCostNode;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        //path.Add(endNode);
        PathNode current = endNode.previousNode;
        while (current != null)
        {
            path.Add(current);
            current = current.previousNode;
        }
        path.Reverse();
        return path;
    }
    
}