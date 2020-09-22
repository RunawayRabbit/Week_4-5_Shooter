using UnityEngine;

public class BezierSpline : MonoBehaviour
{
    // Increase this to improve the accuracy of distance-based interpolation. I recommend 30.
    private const int LUTLength = 80;
    private float[] _distanceLUT;
    [HideInInspector] public Vector3[] anchorPoints;
    [HideInInspector] public Vector3[] controlPoints;
    [SerializeField] public bool isClosed;
    [SerializeField] public Vector3[] points;
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
        var segment = Mathf.FloorToInt(t);

        return GetPointOnSegment(anchorPoints[segment],
            anchorPoints[segment + 1],
            controlPoints[2 * segment],
            controlPoints[2 * segment + 1],
            t - segment);
    }

    private Vector3 GetPointOnSegment(Vector3 start, Vector3 end,
        Vector3 ctrl1, Vector3 ctrl2, float t)
    {
        t = Mathf.Clamp01(t);
        var oneMinusT = 1f - t;

        var startContribution = oneMinusT * oneMinusT * oneMinusT * start;
        var ctrlOneContribution = 3.0f * oneMinusT * oneMinusT * t * ctrl1;
        var ctrlTwoContribution = 3.0f * oneMinusT * t * t * ctrl2;
        var endContribution = t * t * t * end;

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

        for (int i = 0, j = 0;
            i < points.Length - 1;
            i++, j += 2)
        {
            var delta = points[i + 1] - points[i];
            controlPoints[j + 0] = points[i] + delta * oneThird;
            controlPoints[j + 1] = points[i] + delta * twoThirds;
        }

        if (isClosed)
        {
            var finalPoint = points[points.Length - 1];
            var delta = points[0] - finalPoint;
            controlPoints[controlPoints.Length - 2] = finalPoint + delta * oneThird;
            controlPoints[controlPoints.Length - 1] = finalPoint + delta * twoThirds;
        }
    }

    private void MakeAnchorPoints()
    {
        const float oneHalf = 0.5f;

        if (isClosed)
            anchorPoints = new Vector3[points.Length + 1];
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
            i++, j += 2)
            anchorPoints[i] = (controlPoints[j] + controlPoints[j + 1]) * oneHalf;
    }


    public float GetTFromDistance(float distance)
    {
        if (distance <= 0.0f) return 0.0f;
        if (distance >= _distanceLUT[LUTLength - 1]) return 1.0f;

        var LUTIndex = 0;
        var prevDistance = 0.0f;
        while (LUTIndex < LUTLength - 1)
        {
            var currentDistance = _distanceLUT[++LUTIndex];
            if (distance < currentDistance)
            {
                var localT = (distance - prevDistance) / (currentDistance - prevDistance);
                var from = (float) (LUTIndex - 1) / LUTLength;
                var to = (float) LUTIndex / LUTLength;
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

        var scaledT = t * (LUTLength - 1);
        var from = Mathf.FloorToInt(scaledT);
        var to = Mathf.FloorToInt(scaledT + 1.0f);

        if (from < 0)
            return _distanceLUT[0];
        if (to >= LUTLength)
            return _distanceLUT[LUTLength];

        return Mathf.Lerp(_distanceLUT[from], _distanceLUT[to], scaledT - from);
    }

    private void BuildDistanceLUT()
    {
        // Got some help from Freya. Thanks again!
        _distanceLUT = new float[LUTLength];

        var lengthAccumulator = 0.0f;
        var previousPoint = anchorPoints[0];
        for (var i = 1; i < LUTLength; i++)
        {
            var t = i / (float) (LUTLength - 1);
            var point = GetPoint(t);
            var distance = (point - previousPoint).magnitude;
            lengthAccumulator += distance;
            _distanceLUT[i] = lengthAccumulator;
            previousPoint = point;
        }
    }
}