
using UnityEngine;

public class MoveAlongBSpline : MonoBehaviour, IEnemyMover
{
    [SerializeField] private BezierSpline path;
    public void Move()
    {
        transform.position += (transform.forward * Time.deltaTime);
    }
}
