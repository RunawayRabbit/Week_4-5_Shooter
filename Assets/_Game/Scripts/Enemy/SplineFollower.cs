
using UnityEngine;

public class SplineFollower : MonoBehaviour, IEnemyMover
{
    [SerializeField] private GameObject pathObject = default;
    private BezierSpline path;

    [SerializeField] private float Speed = 5.0f;
    [SerializeField] private int numWaypoints = 5;
    [SerializeField]private Vector3 pathOffset = default;

    private int currentWaypoint = 0;
    private Vector3[] waypoints;

    private bool pathEnded = false;

    public void Move()
    {
        if (pathEnded) return;
        const float toleranceSq = 0.1f * 0.1f;
        var currentPosition = transform.position;
        var targetWaypoint = waypoints[currentWaypoint] + pathOffset;

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
    }

    private void Start()
    {
        GenerateWaypoints();
    }

    private void GenerateWaypoints()
    {
        waypoints = new Vector3[numWaypoints];
        float distanceBetweenPoints = path.ArcLength / numWaypoints;
        for (int waypointIndex = 0;
            waypointIndex < numWaypoints;
            waypointIndex++)
        {
            float distance = (float)waypointIndex * distanceBetweenPoints;
            float t = path.GetTFromDistance(distance);
            waypoints[waypointIndex] = path.GetWorldPoint(t);
        }
    }
}
