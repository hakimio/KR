using System;

public struct Point
{
    public int X, Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return String.Format("({0}, {1})", X, Y);
    }
}
