using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Tile: GridObject, IHasNeighbours<Tile>
{
    public bool Passable;

    public Tile(int x, int y)
        : base(x, y)
    {
        Passable = true;
    }

    public IEnumerable<Tile> AllNeighbours { get; set; }
    public IEnumerable<Tile> Neighbours
    {
        get { return AllNeighbours.Where(o => o.Passable); }
    }

    public void FindNeighbours(Dictionary<Point, TileBehaviour> Board,
        Vector2 BoardSize, bool EqualLineLengths)
    {
        List<Tile> neighbours = new List<Tile>();

        foreach (Point point in NeighbourShift)
        {
            int neighbourX = X + point.X;
            int neighbourY = Y + point.Y;
            int xOffset = neighbourY / 2;

            if (neighbourY % 2 != 0 && !EqualLineLengths &&
                neighbourX + xOffset == BoardSize.x - 1)
                continue;

            if (neighbourX >= 0 - xOffset &&
                neighbourX < (int)BoardSize.x - xOffset &&
                neighbourY >= 0 && neighbourY < (int)BoardSize.y)
                neighbours.Add(Board[new Point(neighbourX, neighbourY)].tile);
        }

        AllNeighbours = neighbours;
    }

    public static List<Point> NeighbourShift
    {
        get
        {
            return new List<Point>
                {
                    new Point(0, 1),
                    new Point(1, 0),
                    new Point(1, -1),
                    new Point(0, -1),
                    new Point(-1, 0),
                    new Point(-1, 1),
                };
        }
    }
}
