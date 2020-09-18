using System.Collections.Generic;
using UnityEngine;

public abstract class CameraBehaviour : ICameraBehaviour
{
    protected Vector3 displacement = default;
    protected float rotateSpeed = 3.0f;
    protected float smoothTime = 0.1f;
    protected float tiltAmount = 3.0f;

    protected List<GameObject> _followTargets;
    protected List<GameObject> _lookTargets;
    protected Vector3 _velocity;

    public CameraBehaviour(CamAttributes camData, GameObject followTarget, GameObject lookTarget)
    {
        displacement = camData.displacement;
        rotateSpeed = camData.rotateSpeed;
        smoothTime = camData.smoothTime;
        tiltAmount = camData.tiltAmount;

        _followTargets = new List<GameObject>();
        _lookTargets = new List<GameObject>();

        _followTargets.Add(followTarget);
        _lookTargets.Add(lookTarget);
    }

    public CameraBehaviour(CamAttributes camData, List<GameObject> followTargets, List<GameObject> lookTargets)
    {
        displacement = camData.displacement;
        rotateSpeed = camData.rotateSpeed;
        smoothTime = camData.smoothTime;
        tiltAmount = camData.tiltAmount;

        _followTargets = followTargets;
        _lookTargets = lookTargets;
    }

    public abstract Vector3 GetPosition(Vector3 inPosition);

    public abstract Quaternion GetRotation(Transform transform);

    public abstract float GetFoV(float mainFieldOfView);

    protected Vector3 AggregateTargets(List<GameObject> targets)
    {
        if (targets.Count == 1)
            return targets[0].transform.localPosition;

        var bounds = new Bounds();
        foreach (var target in targets)
            bounds.Encapsulate(target.transform.localPosition);

        return bounds.center;
    }
}