using System.Linq;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    Vector2 _firsRectanglePoint = new Vector2(4, 2);
    Vector2 _secondRectanglePoint = new Vector2(10, 5);
    Vector2 _thirdRectanglePoint = new Vector2(7, 8);
    Vector2 _fourthRectanglePoint = new Vector2(7, 12);
    Vector2 _fifthRectanglePoint = new Vector2(3, 13);
    Vector2 _sixthRectanglePoint = new Vector2(3, 7);
    private void Start()
    {
        var pathFinder = new PathFinder();

        var unitPath = pathFinder.GetPath(_secondRectanglePoint, _fifthRectanglePoint);

        DrawUnitPath(unitPath.ToArray());
    }

    private void DrawUnitPath(Vector2[] points)
    {

        var previousPoint = points[0];
        for (int i = 1; i < points.Length; i++)
        {
            Debug.DrawLine(previousPoint, points[i], Color.green, 60);
            previousPoint = points[i];
        }
    }
}
