using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [SerializeField] private float minPointsDistance;

    [HideInInspector] public List<Vector3> points = new();
    [HideInInspector] public int pointsCount = 0;

    private float _pointFixedYAxis;


    private void Start()
    {
        _pointFixedYAxis = lineRenderer.GetPosition(0).y;
        Clear();
    }

    public void Initialize()
    {
        gameObject.SetActive(true);
    }
    public void Clear()
    {
        gameObject.SetActive(false);
        lineRenderer.positionCount = 0;
        pointsCount = 0;
        points.Clear();
    }
    public void AddPoint(Vector3 newPoint)
    {
        newPoint.y = _pointFixedYAxis;
        if (pointsCount >= 1 && Vector3.Distance(newPoint, GetLastPoint()) < minPointsDistance)
            return;
        
        
        points.Add(newPoint);
        pointsCount++;

        // linerenderer update : 
        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPosition(pointsCount - 1, newPoint);
    }

    private Vector3 GetLastPoint()
    {
        return lineRenderer.GetPosition(pointsCount - 1);
    }
    public void SetColor (Color color)
    {
        lineRenderer.sharedMaterial.color = color;
    }
}
