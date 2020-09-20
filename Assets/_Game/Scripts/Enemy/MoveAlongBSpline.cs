
using UnityEngine;

public class MoveAlongBSpline : MonoBehaviour, IEnemyMover
{
    private BezierSpline path;
    private (float, float)[] _distanceLUT;
    public void Move()
    {
        transform.position += (transform.forward * Time.deltaTime);
    }

    private void Awake()
    {
        path = GetComponent<BezierSpline>();
        BuildDistanceLUT();
    }

    private void BuildDistanceLUT()
    {
        // Got some help from Freya. Thanks again!
        // @REFERENCE: https://www.geometrictools.com/Documentation/MovingAlongCurveSpecifiedSpeed.pdf

        const int stepCount = 20;
        _distanceLUT = new (float, float)[stepCount];

        float lengthAccumulator = 0.0f;
        Vector3 previousPoint = path.anchorPoints[0];
        for (int i = 1; i < stepCount; i++)
        {
            float t = ((float) i) / (stepCount - 1.0f);
            Vector3 point = path.GetWorldPoint(t);
            float distance = (previousPoint - point).magnitude;
            lengthAccumulator += distance;
            _distanceLUT[i] = (t, lengthAccumulator);
            
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = point;

        }

        /*
        const int stepCount = 20;
        _distanceLUT = new List<(float, float)>(stepCount);
        
        var readonly stepAmount = 
        var accumulator = 0;
        for (int i = 0; i < stepCount - 1; i++)
        {
            accumulator += Float
        } 
*/
    }
}
