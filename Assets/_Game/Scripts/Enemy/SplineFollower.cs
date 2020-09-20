
using UnityEngine;

public class SplineFollower : MonoBehaviour, IEnemyMover
{
    [SerializeField] private GameObject pathObject;
    private BezierSpline path;

    [SerializeField] private float Speed = 5.0f;
    [SerializeField] private int numWaypoints = 5;

    private int currentWaypoint = 0;
    private Vector3[] waypoints;

    // Increase this to improve the accuracy of distance-based interpolation. I recommend 30.
    private const int LUTLength = 30;
    private float[] _distanceLUT;
    
    private bool pathEnded = false;

    public void Move()
    {
        if (pathEnded) return;
        const float toleranceSq = 0.1f * 0.1f;
        var currentPosition = transform.position;
        var targetWaypoint = waypoints[currentWaypoint];

        var velocity = (targetWaypoint - currentPosition).normalized *
                       (Speed * Time.deltaTime);
        
        if ((currentPosition + velocity - targetWaypoint).sqrMagnitude < toleranceSq)
        {
            currentWaypoint++;
            if (currentWaypoint == numWaypoints)
            {
                if (path.isClosed)
                    currentWaypoint = 0;
                else
                    pathEnded = true;
            }
        }
        
        transform.position += velocity;
    }

    //@TODO: This was a cool mistake. Make it into a different movement mode!
    public void SteppyMove()
    {
        if (pathEnded) return;
        const float toleranceSq = 0.1f * 0.1f;
        var currentPosition = transform.position;
        var nextWaypoint = waypoints[currentWaypoint + 1];

        var velocity = (nextWaypoint - currentPosition) * (Speed * Time.deltaTime);
        if ((currentPosition + velocity - nextWaypoint).sqrMagnitude < toleranceSq)
        {
            currentWaypoint++;
            if (currentWaypoint == numWaypoints)
            {
                if (path.isClosed)
                    currentWaypoint = 0;
                else
                    pathEnded = true;
            }
        }
        
        transform.position += velocity;
    }

    private void Awake()
    {
        if(!pathObject.TryGetComponent<BezierSpline>(out path))
            Debug.LogWarning($"SplineFollower on {gameObject.name} doesn't have a spline object defined! I have nothing to follow!");
    
        BuildDistanceLUT();
        GenerateWaypoints();
    }

    private void GenerateWaypoints()
    {
        waypoints = new Vector3[numWaypoints];
        float distanceBetweenPoints = _distanceLUT[LUTLength - 1] / numWaypoints;
        for (int waypointIndex = 0;
            waypointIndex < numWaypoints;
            waypointIndex++)
        {
            float distance = (float)waypointIndex * distanceBetweenPoints;
            float t = GetTFromDistance(distance);
            waypoints[waypointIndex] = path.GetWorldPoint(t);
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
            previousPoint = point;
        }
    }
}
