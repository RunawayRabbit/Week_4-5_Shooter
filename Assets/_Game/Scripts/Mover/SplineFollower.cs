
using JetBrains.Annotations;
using UnityEngine;

public class SplineFollower : MonoBehaviour, IMover
{
    [CanBeNull, SerializeField] private GameObject pathObject = default;
    private BezierSpline _path;

    [SerializeField] private bool localSpace = false;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private int numWaypoints = 5;
    [SerializeField] private Vector3 pathOffset = default;

    private int _currentWaypoint = 0;
    [CanBeNull] private Vector3[] _waypoints;

    private bool _pathEnded = false;
    

    private void Awake()
    {
        if (TryGetComponent<BezierSpline>(out _path))
            pathObject = gameObject;
        else if(!pathObject || !pathObject.TryGetComponent<BezierSpline>(out _path))
        {
            Debug.LogWarning($"SplineFollower on {gameObject.transform.name} doesn't have a spline object defined! I have nothing to follow!");
            return;
        }
        
        GenerateWaypoints();
    }
    public Vector3 Move()
    {
        if (_pathEnded || _waypoints == null) return Vector3.zero;
        if (_waypoints.Length == 0)
        {
            Debug.Log("We didn't build a waypoint array yet!");
            return Vector3.zero;
        }
        const float toleranceSq = 0.1f * 0.1f;
        var currentPosition = localSpace ? transform.localPosition : transform.position;
        var targetWaypoint = _waypoints[_currentWaypoint] + pathOffset;

        var velocity = (targetWaypoint - currentPosition).normalized *
                       (speed * Time.deltaTime);
        
        if ((currentPosition + velocity - targetWaypoint).sqrMagnitude < toleranceSq)
        {
            _currentWaypoint++;
            if (_currentWaypoint == numWaypoints)
            {
                if (_path.isClosed)
                    _currentWaypoint = 0;
                else
                    _pathEnded = true;
            }
        }

        if (localSpace)
            transform.localPosition += velocity;
        else
            transform.position += velocity;
        return velocity;
    }

    //@TODO: This was a cool mistake. Make it into a different movement mode!
    public void SteppyMove()
    {
        if (_pathEnded) return;
        const float toleranceSq = 0.1f * 0.1f;
        var currentPosition = transform.position;
        var nextWaypoint = _waypoints[_currentWaypoint + 1];

        var velocity = (nextWaypoint - currentPosition) * (speed * Time.deltaTime);
        if ((currentPosition + velocity - nextWaypoint).sqrMagnitude < toleranceSq)
        {
            _currentWaypoint++;
            if (_currentWaypoint == numWaypoints)
            {
                if (_path.isClosed)
                    _currentWaypoint = 0;
                else
                    _pathEnded = true;
            }
        }

        transform.position += velocity;
    }
    
    private void GenerateWaypoints()
    {
        _waypoints = new Vector3[numWaypoints];
        float distanceBetweenPoints = _path.ArcLength / numWaypoints;
        for (int waypointIndex = 0;
            waypointIndex < numWaypoints;
            waypointIndex++)
        {
            float distance = (float)waypointIndex * distanceBetweenPoints;
            float t = _path.GetTFromDistance(distance);
            _waypoints[waypointIndex] = _path.GetWorldPoint(t);
        }
    }
}
