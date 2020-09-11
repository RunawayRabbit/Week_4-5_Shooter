using UnityEngine;
/*
public class CameraTransition : CameraBehaviour
{
    private CameraBehaviour _from;
    private CameraBehaviour _to;

    private float currentTime = 0.0f;
    private float finishTime = default;

    private CameraTransition() { } //@TODO is this necessary?
    public CameraTransition(CameraBehaviour from, CameraBehaviour to, float time)
    {
        _from = from;
        _to = to;
        finishTime = time;
    }
    
    public override Vector3 GetPosition()
    {
        Vector3 from = _from.GetPosition();
        Vector3 to = _to.GetPosition();
        float t = currentTime / finishTime;

        Vector3 newPosition = new Vector3(
            Mathf.SmoothStep(from.x, to.x, t),
            Mathf.SmoothStep(from.y, to.y, t),
            Mathf.SmoothStep(from.z, to.z, t));

        return newPosition;
    }
}*/