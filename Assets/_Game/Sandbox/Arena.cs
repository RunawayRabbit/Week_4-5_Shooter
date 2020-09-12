using UnityEngine;

public class Arena : MonoBehaviour
{
    public enum Mode
    {
        Horizontal,
        Vertical
    }
    public Mode CurrentMode { get; private set; }

    public Vector2 horizontalArena;
    public Vector2 verticalArena;
    private Vector2 CurrentArena => (CurrentMode == Mode.Horizontal) ? horizontalArena : verticalArena;
    
    //@NOTE: This code requires that our arenas always be axis-aligned vertical and horizontal.
    public static Vector3 BasisX = Vector3.right;
    public Vector3 BasisY => (CurrentMode == Mode.Horizontal) ? Vector3.forward : Vector3.up;
    public Vector3 CurrentNormal => (CurrentMode == Mode.Horizontal) ? Vector3.up : Vector3.back;

    private void Awake()
    {
        CurrentMode = Mode.Horizontal;
    }
    
    // ReSharper disable once UnusedMember.Global
    public void ToggleMode()
    {   
        CurrentMode = CurrentMode == Mode.Horizontal ? Mode.Vertical : Mode.Horizontal; 
        Debug.Log($"MODE SWITCHED: {CurrentMode}");
    }

    public float ForwardDistanceToBounds(Vector3 point, Vector2 inDirection)
    {
        Vector3 direction = Convert2Dto3D(inDirection);
        direction.Normalize();

        Vector3 planeOffsetX = new Vector3(CurrentArena.x, 0.0f, 0.0f);
        Vector3 planeOffsetY = (CurrentMode == Mode.Horizontal)
            ? new Vector3(0.0f, 0.0f, CurrentArena.y)
            : new Vector3(0.0f, CurrentArena.y, 0.0f); 
        
        //@TODO: 90% confident that planeOffsetY and BasisY are correct. TEST TO BE SURE!
        var planeDistances = new float[4]
        {
            LineToPlaneIntersection(point, direction, planeOffsetX, BasisX),
            LineToPlaneIntersection(point, direction, -planeOffsetX, -BasisX),
            LineToPlaneIntersection(point, direction, planeOffsetY, BasisY),
            LineToPlaneIntersection(point, direction, -planeOffsetY, -BasisY)
        };

        float closestDistance = float.PositiveInfinity;
        foreach (var distance in planeDistances)
        {
            if (distance < closestDistance && distance > 0.0f) closestDistance = distance;
        }

        return closestDistance;
    }

    // Returns X where point + X*direction is on the plane.
    private float LineToPlaneIntersection(Vector3 point, Vector3 direction, Vector3 planeOrigin, Vector3 planeNormal)
    {
        float topHalf = Vector3.Dot((planeOrigin - point), planeNormal);
        float bottomHalf = Vector3.Dot(direction, planeNormal);

        return topHalf / bottomHalf;
    }

    private float DistanceFromPlane(Vector3 incomingPoint, Vector3 planeNormal, float planeOffset)
    {
        return Vector3.Dot(incomingPoint, planeNormal) - planeOffset;
    }
    
    public Vector3 ProjectPointToArena(Vector3 incomingPoint, float planeOffset = 0.0f)
    {
        //@NOTE: We are assuming that our normal vector is normalized.
        float distanceFromPlane = DistanceFromPlane(incomingPoint, CurrentNormal, 0.0f);
        Vector3 pointProjectedOnPlane = incomingPoint - CurrentNormal * distanceFromPlane;

        return pointProjectedOnPlane;
    }

    public Vector3 Convert2Dto3D(Vector2 inVector)
    {
        return Convert2Dto3D(inVector, Vector3.zero);
    }
    public Vector3 Convert2Dto3D(Vector2 inVector, Vector3 currentPosition)
    {
        return (CurrentMode == Mode.Horizontal)
            ? new Vector3(inVector.x, -currentPosition.y, inVector.y)
            : new Vector3(inVector.x, inVector.y, -currentPosition.z);
    }

    public Vector3 ConstrainToBounds(Vector3 inPosition)
    {
        Vector3 outPosition;
        outPosition.x = Mathf.Clamp(inPosition.x, -CurrentArena.x, CurrentArena.x);
        if (CurrentMode == Mode.Horizontal)
        {
            outPosition.y = inPosition.y;
            outPosition.z = Mathf.Clamp(inPosition.z, -CurrentArena.y, CurrentArena.y);
        }
        else
        {
            outPosition.y = Mathf.Clamp(inPosition.y, -CurrentArena.y, CurrentArena.y);
            outPosition.z = inPosition.z;
        }

        return outPosition;
    }

    public Vector3 DirectionToPlane(Vector3 inPosition)
    {
        return CurrentMode == Mode.Horizontal ?
            inPosition.y < 0.0f ? Vector3.up : Vector3.down :
            inPosition.z < 0.0f ? Vector3.forward : Vector3.back;
    }
}
