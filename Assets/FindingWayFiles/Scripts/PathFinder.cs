using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : IPathFinder
{
    private Dictionary<Rectangle, List<Rectangle>> _recatanglesNaighbors = new Dictionary<Rectangle, List<Rectangle>>();

    private bool isRectangleContainsPoint(Rectangle rectangle, Vector2 point) => (point.x >= rectangle.Min.x) && (point.x <= rectangle.Max.x)
                                                                                && (point.y >= rectangle.Min.y) && (point.y <= rectangle.Max.y);
    private Edge[] _edges;


    public PathFinder()
    {
        var rec1 = new Rectangle { Min = new Vector2(1, 1), Max = new Vector2(8, 4) };
        var rec2 = new Rectangle { Min = new Vector2(8, 3), Max = new Vector2(11, 8) };
        var rec3 = new Rectangle { Min = new Vector2(5, 6), Max = new Vector2(8, 11) };
        var rec4 = new Rectangle { Min = new Vector2(6, 11), Max = new Vector2(13, 13) };
        var rec5 = new Rectangle { Min = new Vector2(2, 10), Max = new Vector2(5, 14) };
        var rec6 = new Rectangle { Min = new Vector2(2, 4), Max = new Vector2(4, 10) };

        var edge1 = new Edge(rec1, rec2, new Vector2(4, 2), new Vector2(10, 5));
        var edge5 = new Edge(rec1, rec6, new Vector2(4, 2), new Vector2(3, 7));
        var edge2 = new Edge(rec2, rec3, new Vector2(10, 5), new Vector2(7, 8));
        var edge3 = new Edge(rec3, rec4, new Vector2(7, 8), new Vector2(7, 12));
        var edge4 = new Edge(rec3, rec5, new Vector2(7, 8), new Vector2(3, 13));
        var edge6 = new Edge(rec5, rec6, new Vector2(3, 13), new Vector2(3, 7));

        _edges = new Edge[] { edge1, edge2, edge3, edge4, edge5, edge6 };
    }

    public IEnumerable<Vector2> GetPath(Vector2 A, Vector2 C)
    {
        return GetPath(A, C, _edges);
    }

    public IEnumerable<Vector2> GetPath(Vector2 A, Vector2 C, IEnumerable<Edge> edges)
    {
        var edgesArray = edges.ToArray();
        var rectangles = GetAllRectangles(edgesArray);

        var rectangleA = GetRectangleWithPoint(rectangles, A);
        var rectangleC = GetRectangleWithPoint(rectangles, C);
        if (rectangleA == null || rectangleC == null)
        {
            Debug.LogError("One of points is out of rectangles");
            return new List<Vector2>();
        }

        if (rectangleA == rectangleC)
        {
            return new Vector2[] { A, C };
        }

        _recatanglesNaighbors = FindRectangleNaighbors(edgesArray, rectangles);
        var rectanglePath = GenerateRectanglesPath(rectangleA.Value, rectangleC.Value);
        if (rectanglePath == null)
        {
            Debug.LogError("Path can not be found ");
            return new List<Vector2>();
        }

        var path = GeneratePathBetweenRectangles(rectanglePath.ToArray(), edgesArray);

        DrawRectangles(rectangles);

        return path;

    }

    private Rectangle[] GetAllRectangles(Edge[] edges)
    {
        var recatngles = new List<Rectangle>();
        foreach (var edge in edges)
        {
            recatngles.Add(edge.First);
            recatngles.Add(edge.Second);
        }
        return recatngles.Distinct().ToArray();
    }

    private Rectangle? GetRectangleWithPoint(Rectangle[] rectangles, Vector2 point)
    {
        foreach (var rect in rectangles)
        {
            if (isRectangleContainsPoint(rect, point))
            {
                return rect;
            }
        }
        return null;

    }

    private Dictionary<Rectangle, List<Rectangle>> FindRectangleNaighbors(Edge[] edges, Rectangle[] rectangles)
    {
        var recatangleNaighbors = new Dictionary<Rectangle, List<Rectangle>>();
        foreach (var rectangle in rectangles)
        {
            recatangleNaighbors.Add(rectangle, new List<Rectangle>());
        }

        foreach (var edge in edges)
        {
            recatangleNaighbors[edge.First].Add(edge.Second);
            recatangleNaighbors[edge.Second].Add(edge.First);
        }
        return recatangleNaighbors;
    }

    private List<Rectangle> GenerateRectanglesPath(Rectangle rectangleA, Rectangle rectangleC)
    {
        var checkedRectangles = new List<Rectangle>();
        var rectanglesWeight = new Dictionary<Rectangle, int>(_recatanglesNaighbors.Count);
        var recatnglesToCheck = new List<Rectangle> { rectangleC };
        var isPathFound = false;

        foreach (var rectangle in _recatanglesNaighbors.Keys)
        {
            rectanglesWeight.Add(rectangle, -1);
        }
        rectanglesWeight[rectangleC] = 0;

        while (recatnglesToCheck.Count != 0 && !isPathFound)
        {
            var newRecatanglesToCheck = new List<Rectangle>();
            foreach (var checkRectangle in recatnglesToCheck)
            {
                foreach (var rectangle in _recatanglesNaighbors[checkRectangle])
                {
                    if (checkedRectangles.Contains(rectangle))
                    {
                        continue;
                    }

                    if (rectanglesWeight[rectangle] == -1 ||
                        rectanglesWeight[rectangle] >= rectanglesWeight[checkRectangle])
                    {
                        rectanglesWeight[rectangle] = rectanglesWeight[checkRectangle] + 1;

                        if (rectangle == rectangleA)
                        {
                            isPathFound = true;
                            break;
                        }
                        else
                        {
                            newRecatanglesToCheck.Add(rectangle);
                        }
                    }

                }

                if (isPathFound)
                {
                    break;
                }

                checkedRectangles.Add(checkRectangle);
            }

            recatnglesToCheck = newRecatanglesToCheck;
        }

        if (!isPathFound)
        {
            return null;
        }

        var currentRectangle = rectangleA;
        var recatanglePath = new List<Rectangle>() { rectangleA };
        while (currentRectangle != rectangleC)
        {
            var rectangleNaighbors = _recatanglesNaighbors[currentRectangle];
            var requiredRectangleWeight = rectanglesWeight[currentRectangle] - 1;

            foreach (var rectangle in rectangleNaighbors)
            {
                if (rectanglesWeight[rectangle] == requiredRectangleWeight)
                {
                    recatanglePath.Add(rectangle);
                    currentRectangle = rectangle;
                    break;
                }
            }
        }

        return recatanglePath;
    }

    private List<Vector2> GeneratePathBetweenRectangles(Rectangle[] rectangles, Edge[] edges)
    {
        var path = new List<Vector2>();
        var prevoiusRectangle = rectangles[0];

        for (int i = 1; i < rectangles.Length; i++)
        {
            var edge = edges.Where(e => (e.First == prevoiusRectangle && e.Second == rectangles[i]) ||
                                        (e.First == rectangles[i] && e.Second == prevoiusRectangle)).ToArray();
            if (!path.Contains(edge[0].Start))
            {
                path.Add(edge[0].Start);
            }
            if (!path.Contains(edge[0].End))
            {
                path.Add(edge[0].End);
            }
            prevoiusRectangle = rectangles[i];
        }

        return path;
    }

    public void DrawRectangles(Rectangle[] rectangles)
    {
        foreach (Rectangle rect in rectangles)
        {
            var min = rect.Min;
            var max = rect.Max;
            var upperLeftCorner = new Vector3(min.x, max.y, 0);
            var lowerRightCorner = new Vector3(max.x, min.y, 0);
            Debug.DrawLine(min, upperLeftCorner, Color.red, 60);
            Debug.DrawLine(min, lowerRightCorner, Color.red, 60);
            Debug.DrawLine(max, upperLeftCorner, Color.red, 60);
            Debug.DrawLine(max, lowerRightCorner, Color.red, 60);

        }
    }
}
