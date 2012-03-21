using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public static class GridHelper
{
    public static List<Tile> getTilesWithDistance(Tile originTile, int distance)
    {
        if (distance == 0)
            return new List<Tile>();

        HashSet<Tile> closedSet = new HashSet<Tile>();
        Queue<Tile> openSet = new Queue<Tile>();
        List<Tile> result = new List<Tile>();

        foreach (Tile neighbour in originTile.Neighbours)
                openSet.Enqueue(neighbour);
        closedSet.Add(originTile);

        while(openSet.Count > 0)
        {
            Tile tileToCheck = openSet.Dequeue();
            var path = PathFinder.FindPath(originTile, tileToCheck);
            closedSet.Add(tileToCheck);
            if (path.ToList().Count - 1 <= distance)
            {
                result.Add(tileToCheck);
                    
                foreach (Tile tile in tileToCheck.Neighbours)
                    if (!closedSet.Contains(tile) && !openSet.Contains(tile))
                        openSet.Enqueue(tile);
            }
        }

        return result;
    }
}
