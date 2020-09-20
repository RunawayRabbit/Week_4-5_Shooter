
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private Vector3[] points;
    [SerializeField] private bool isClosed;
    [SerializeField] private Vector3[] _controlPoints;
    [SerializeField] private Vector3[] _anchorPoints;

    private void Awake()
    {
        CalculatePoints();
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        CalculatePoints();
    }
#endif

    private void CalculatePoints()
    {
        MakeControlPoints();
        MakeAnchorPoints();
    }
    
    private void MakeControlPoints()
    {
        const float oneThird = 1.0f / 3.0f;
        const float twoThirds = 2.0f / 3.0f;

        if (isClosed)
            _controlPoints = new Vector3[points.Length * 2];
        else 
            _controlPoints = new Vector3[(points.Length - 1) * 2];

        for (int i = 0,  j = 0;
            i < points.Length-1;
            i++, j += 2)
        {
            Vector3 delta = points[i + 1] - points[i];
            _controlPoints[j + 0] = points[i] + delta * oneThird;
            _controlPoints[j + 1] = points[i] + delta * twoThirds;
        }

        if (isClosed)
        {
            Vector3 finalPoint = points[points.Length - 1];
            Vector3 delta = points[0] - finalPoint;
            _controlPoints[_controlPoints.Length - 2] = finalPoint + delta * oneThird;
            _controlPoints[_controlPoints.Length - 1] = finalPoint + delta * twoThirds;
        }
    }

    private void MakeAnchorPoints()
    {
        const float oneHalf = 0.5f;
        
        if (isClosed)
            _anchorPoints = new Vector3[points.Length+1];
        else
            _anchorPoints = new Vector3[points.Length];

        if (isClosed)
        {
            // special case endpoints
            _anchorPoints[0] =
                (_controlPoints[0] + _controlPoints[_controlPoints.Length - 1]) * oneHalf;
            
            // @TODO: Hacky solution, probably can be improved..
            _anchorPoints[_anchorPoints.Length - 1] = _anchorPoints[0];
        }
        else
        {
            _anchorPoints[0] = points[0];
            _anchorPoints[_anchorPoints.Length - 1] = points[points.Length - 1];
        }

        for (int i = 1, j = 1;
            i < _anchorPoints.Length - 1;
            i++, j+=2)
        {
            _anchorPoints[i] = (_controlPoints[j] + _controlPoints[j + 1]) * oneHalf;
        }
    }
}
