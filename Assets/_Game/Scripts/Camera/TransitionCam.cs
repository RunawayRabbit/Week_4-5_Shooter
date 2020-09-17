using UnityEngine;
public class TransitionCam : ICameraBehaviour
{
    private ICameraBehaviour _from;
    private ICameraBehaviour _to;

    private readonly float startTime;
    private readonly float lifetime;
    
    public TransitionCam(ICameraBehaviour from, ICameraBehaviour to, float lifetime)
    {
        _from = from;
        _to = to;
        startTime = Time.time;
        this.lifetime = lifetime;
    }

    private float HandleCompletion(CameraController cam)
    {
        float elapsed = Time.time - startTime;
        float t = elapsed / lifetime;
        if (!(t >= 1.0f)) return t;
        
        cam._camBehaviour = _to;
        return 1.0f;
    }
    
    public Vector3 GetPosition(CameraController cam)
    {
        float t = HandleCompletion(cam);
        Debug.Log(t);
        return Vector3.Lerp(_from.GetPosition(cam), _to.GetPosition(cam), t);
    }

    public Quaternion GetRotation(CameraController cam)
    {
        float t = HandleCompletion(cam);
        return Quaternion.Lerp(_from.GetRotation(cam), _to.GetRotation(cam), t);
    }

    public float GetFoV(CameraController cam, float currentFoV)
    {
        float t = HandleCompletion(cam);
        return Mathf.Lerp(_from.GetFoV(cam, currentFoV), _to.GetFoV(cam, currentFoV), t);
    }
}
