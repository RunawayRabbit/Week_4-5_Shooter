using UnityEngine;

#if false
public class TransitionCam : ICameraBehaviour
{
    private readonly ICameraBehaviour _from;
    private readonly ICameraBehaviour _to;

    private readonly float _startTime;
    private readonly float _lifetime;
    private float _fovVelocity = 0.0f;
    private Vector3 _velocity = Vector3.zero;

    public TransitionCam(ICameraBehaviour from, ICameraBehaviour to, float lifetime)
    {
        _from = from;
        _to = to;
        _startTime = Time.time;
        this._lifetime = lifetime;
    }

    private float GetRemappedT(float t)
    {
        return t * t * (3 - 2 * t); // Thanks Freya!
    }

    public void MoveCamera(CameraController cam, ref Vector3 position, ref Quaternion rotation, ref float fov)
    {
        float elapsed = Time.time - _startTime;
        float t = elapsed / _lifetime;
        if (t >= 1.0f)
        {
            _to.MoveCamera(cam, ref position, ref rotation, ref fov);
            cam._camBehaviour = _to;
            Debug.Log("Transition Over");
            return;
        }

        t = GetRemappedT(t);
        
        var fromPos = position;
        var toPos = position;
        var fromRot = rotation;
        var toRot = rotation;
        var fromFov = fov;
        var toFov = fov;
        
        _from.MoveCamera(cam, ref fromPos, ref fromRot, ref fromFov);
        _to.MoveCamera(cam, ref toPos, ref toRot, ref toFov);

        position = Vector3.Lerp(fromPos, toPos, t);
        rotation = Quaternion.Slerp(fromRot, toRot, t);
        
        cam
        //fov = Mathf.Lerp(fromFov, toFov, t);
    }
}
#endif
