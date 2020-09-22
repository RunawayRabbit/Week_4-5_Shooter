﻿
using UnityEngine;

public class BezierSpline : MonoBehaviour
{
    [SerializeField] public bool isClosed;
    [SerializeField] public Vector3[] points;
    [HideInInspector] public Vector3[] controlPoints;
    [HideInInspector] public Vector3[] anchorPoints;

    // Increase this to improve the accuracy of distance-based interpolation. I recommend 30.
    private const int LUTLength = 50;
    private float[] _distanceLUT;
    public float ArcLength => _distanceLUT[LUTLength - 1];
    private void Awake()
    {
        CalculatePoints();
        BuildDistanceLUT();
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        CalculatePoints();
    }
#endif

    public Vector3 GetWorldPoint(float t)
    {
       return transform.TransformPoint(GetPoint(t));
    }

    public Vector3 GetPoint(float t)
    {
        t = t * anchorPoints.Length;
        if (t < 0.0f) t = 0.0f;
        if (t >= anchorPoints.Length - 1) return anchorPoints[anchorPoints.Length - 1];
        int segment = Mathf.FloorToInt(t);

        return GetPointOnSegment(anchorPoints[segment],
            anchorPoints[segment + 1],
            controlPoints[2 * segment],
            controlPoints[(2 * segment) + 1],
            (float)(t - segment));
    }

    private Vector3 GetPointOnSegment(Vector3 start, Vector3 end,
        Vector3 ctrl1, Vector3 ctrl2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;

        Vector3 startContribution = oneMinusT * oneMinusT * oneMinusT * start;
        Vector3 ctrlOneContribution = 3.0f * oneMinusT * oneMinusT * t * ctrl1;
        Vector3 ctrlTwoContribution = 3.0f * oneMinusT * t * t * ctrl2;
        Vector3 endContribution = t * t * t * end;

        return startContribution + ctrlOneContribution + ctrlTwoContribution + endContribution;
    }
    
    public void CalculatePoints()
    {
        if (points.Length == 0) return;
        MakeControlPoints();
        MakeAnchorPoints();
    }
    
    private void MakeControlPoints()
    {
        const float oneThird = 1.0f / 3.0f;
        const float twoThirds = 2.0f / 3.0f;

        if (isClosed)
            controlPoints = new Vector3[points.Length * 2];
        else 
            controlPoints = new Vector3[(points.Length - 1) * 2];

        for (int i = 0,  j = 0;
            i < points.Length - 1;
            i++, j += 2)
        {
            Vector3 delta = points[i + 1] - points[i];
            controlPoints[j + 0] = points[i] + delta * oneThird;
            controlPoints[j + 1] = points[i] + delta * twoThirds;
        }

        if (isClosed)
        {
            Vector3 finalPoint = points[points.Length - 1];
            Vector3 delta = points[0] - finalPoint;
            controlPoints[controlPoints.Length - 2] = finalPoint + delta * oneThird;
            controlPoints[controlPoints.Length - 1] = finalPoint + delta * twoThirds;
        }
    }

    private void MakeAnchorPoints()
    {
        const float oneHalf = 0.5f;
        
        if (isClosed)
            anchorPoints = new Vector3[points.Length+1];
        else
            anchorPoints = new Vector3[points.Length];

        if (isClosed)
        {
            // special case endpoints
            anchorPoints[0] =
                (controlPoints[0] + controlPoints[controlPoints.Length - 1]) * oneHalf;
            
            anchorPoints[anchorPoints.Length - 1] = anchorPoints[0];
        }
        else
        {
            anchorPoints[0] = points[0];
            anchorPoints[anchorPoints.Length - 1] = points[points.Length - 1];
        }
        for (int i = 1, j = 1;
            i < anchorPoints.Length - 1;
            i++, j+=2)
        {
            anchorPoints[i] = (controlPoints[j] + controlPoints[j + 1]) * oneHalf;
        }
    }


    public float GetTFromDistance(float distance)
    {
        if (distance <= 0.0f) return 0.0f;
        if (distance >= _distanceLUT[LUTLength - 1]) return 1.0f;
        
        int LUTIndex = 0;
        float prevDistance = 0.0f;
        while (LUTIndex < LUTLength - 1)
        {
            float currentDistance = _distanceLUT[++LUTIndex];
            if (distance < currentDistance)
            {
                float localT = (distance - prevDistance) / (currentDistance - prevDistance);
                float from = (float)(LUTIndex - 1) / LUTLength;
                float to = (float)LUTIndex / LUTLength;
                return Mathf.Lerp(from, to, localT);
            }
            prevDistance = currentDistance;
        }
        return 1.0f;
    }
    
    private float GetDistanceFromT(float t)
    {
        if (_distanceLUT.Length <= 1)
        {
            Debug.LogError("We just tried to sample an empty LUT.");
            return -1.0f;
        }

        float scaledT = t * (LUTLength - 1);
        int from = Mathf.FloorToInt(scaledT);
        int to = Mathf.FloorToInt(scaledT + 1.0f);

        if(from < 0)
            return _distanceLUT[0];
        if (to >= LUTLength)
            return _distanceLUT[LUTLength];

        return Mathf.Lerp(_distanceLUT[from], _distanceLUT[to], scaledT - from);
    }
    
    private void BuildDistanceLUT()
    {
        // Got some help from Freya. Thanks again!
        _distanceLUT = new float[LUTLength];

        float lengthAccumulator = 0.0f;
        Vector3 previousPoint = anchorPoints[0];
        for (int i = 1; i < LUTLength; i++)
        {
            float t = ((float) i) / (float)(LUTLength - 1);
            Vector3 point = GetPoint(t);
            float distance = (point - previousPoint).magnitude;
            lengthAccumulator += distance;
            _distanceLUT[i] = lengthAccumulator;
            previousPoint = point;
        }
    }
}