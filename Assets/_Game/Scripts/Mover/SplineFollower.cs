using JetBrains.Annotations;
using UnityEngine;

public class SplineFollower : MonoBehaviour, IMover
{
    private int _currentWaypoint = 0;

    private bool _pathEnded = false;
    [CanBeNull] private Vector3[] _waypoints;

    [SerializeField] private bool localSpace = false;
    [SerializeField] private int numWaypoints = 5;
    private BezierSpline path;
    [CanBeNull] [SerializeField] private GameObject pathObject = default;
    [SerializeField] private Vector3 pathOffset = default;
    [SerializeField] private float speed = 5.0f;

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
                if (path.isClosed)
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

    private void Awake()
    {
        if (TryGetComponent(out path))
        {
            pathObject = gameObject;
        }
        else if (!pathObject || !pathObject.TryGetComponent(out path))
        {
            Debug.Log(
                $"SplineFollower on {gameObject.transform.name} doesn't have a spline object defined! I have nothing to follow!");
            return;
        }

        GenerateWaypoints();
        pathOffset += transform.position - pathObject.transform.position;
    }

    public void SetPathFromObject(GameObject obj)
    {
        if (!obj.TryGetComponent(out path))
        {
            Debug.LogWarning($"SplineFollower on {gameObject.transform.name} was given a bad object, no spline found!");
            return;
        }

        pathObject = obj;
        GenerateWaypoints();
        pathOffset += transform.position - pathObject.transform.position;
    }

    private void GenerateWaypoints()
    {
        _waypoints = new Vector3[numWaypoints];
        var distanceBetweenPoints = path.ArcLength / numWaypoints;
        for (var waypointIndex = 0;
            waypointIndex < numWaypoints;
            waypointIndex++)
        {
            var distance = waypointIndex * distanceBetweenPoints;
            var t = path.GetTFromDistance(distance);
            _waypoints[waypointIndex] = path.GetWorldPoint(t);
        }
    }
}