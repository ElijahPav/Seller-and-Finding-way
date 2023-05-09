using System.Collections.Generic;
using UnityEngine;
public struct Rectangle
{
    public Vector2 Min;
    public Vector2 Max;

    public static bool operator ==(Rectangle rec1, Rectangle rec2)
    {
        return rec1.Min == rec2.Min && rec1.Max == rec2.Max;
    }
    public static bool operator !=(Rectangle rec1, Rectangle rec2)
    {
        return rec1.Min != rec2.Min || rec1.Max != rec2.Max;
    }
}
public struct Edge
{
    public Rectangle First;
    public Rectangle Second;
    public Vector2 Start;
    public Vector2 End;
    public Edge(Rectangle first, Rectangle second, Vector2 start, Vector2 end)
    {
        First = first;
        Second = second;
        Start = start;
        End = end;
    }
}
public interface IPathFinder
{
    IEnumerable<Vector2> GetPath(Vector2 A, Vector2 C, IEnumerable<Edge> edges);
}
