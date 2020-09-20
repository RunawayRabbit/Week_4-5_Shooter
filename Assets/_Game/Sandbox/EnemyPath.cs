
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] public Vector3[] points;
    [SerializeField] public bool isClosed;
    [HideInInspector] public Vector3[] controlPoints;
    [HideInInspector] public Vector3[] anchorPoints;

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

    public Vector3 GetWorldPoint(float t)
    {
       return transform.TransformPoint(GetPoint(t));
    }

    private Vector3 GetPoint(float f)
    {
        //@TODO: Implement.
        Debug.Assert(false, "We didn't write this yet.");
        return Vector3.zero;
    }

    public Vector3 GetPointOnSegment(float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;

        Vector3 startContribution = oneMinusT * oneMinusT * oneMinusT * anchorPoints[0];
        Vector3 ctrlOneContribution = 3.0f * oneMinusT * oneMinusT * t * controlPoints[0];
        Vector3 ctrlTwoContribution = 3.0f * oneMinusT * t * t * controlPoints[1];
        Vector3 endContribution = t * t * t * anchorPoints[1];

        return startContribution + ctrlOneContribution + ctrlTwoContribution + endContribution;
    }
    
    public void CalculatePoints()
    {
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

        //@TODO: Uncomplexify this shitty loop.
        for (int i = 0,  j = 0;
            i < points.Length-1;
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
            
            // @TODO: Hacky solution, probably can be improved..
            anchorPoints[anchorPoints.Length - 1] = anchorPoints[0];
        }
        else
        {
            anchorPoints[0] = points[0];
            anchorPoints[anchorPoints.Length - 1] = points[points.Length - 1];
        }
        //@TODO: Uncomplexify this shitty loop too.
        for (int i = 1, j = 1;
            i < anchorPoints.Length - 1;
            i++, j+=2)
        {
            anchorPoints[i] = (controlPoints[j] + controlPoints[j + 1]) * oneHalf;
        }
    }
}
