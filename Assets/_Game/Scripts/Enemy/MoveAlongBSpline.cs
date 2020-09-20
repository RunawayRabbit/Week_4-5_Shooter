
using UnityEngine;

public class MoveAlongBSpline : MonoBehaviour, IEnemyMover
{
    private BezierSpline path;
    private float[] _distanceLUT;
    private const int LUTLength = 50;
    public void Move()
    {
        transform.position += (transform.forward * Time.deltaTime);
    }

    private void Awake()
    {
        path = GetComponent<BezierSpline>();
        BuildDistanceLUT();

        float distanceBetweenPoints = 5.0f;
        int numberOfPointsToDraw = Mathf.FloorToInt(_distanceLUT[LUTLength - 1] / distanceBetweenPoints);
        for (int i = 0; i < _distanceLUT[LUTLength - 1]; i++)
        {
            float distance = (float)i * distanceBetweenPoints;
            float t = GetTFromDistance(distance);
            Vector3 point = path.GetWorldPoint(t);
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = point;
            sphere.transform.localScale = Vector3.one * 0.2f;
        }
    }

    private float GetTFromDistance(float distance)
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
        Vector3 previousPoint = path.anchorPoints[0];
        for (int i = 1; i < LUTLength; i++)
        {
            float t = ((float) i) / (float)(LUTLength - 1);
            Vector3 point = path.GetPoint(t);
            float distance = (point - previousPoint).magnitude;
            lengthAccumulator += distance;
            _distanceLUT[i] = lengthAccumulator;
        }
    }
}
