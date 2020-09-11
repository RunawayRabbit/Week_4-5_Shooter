
using System.Collections.Generic;
using UnityEngine;

public class OverShoulderCam : CameraBehaviour
{
    public OverShoulderCam(CamBehaviourData camData, GameObject followTarget, GameObject lookTarget) : base(camData,
        followTarget, lookTarget) { }

    public OverShoulderCam(CamBehaviourData camData, List<GameObject> followTargets, List<GameObject> lookTargets) : base(camData, followTargets, lookTargets) {}

    override public Quaternion GetRotation(Transform inTransform)
    {
        Vector3 lookPosition = base.AggregateTargets(_lookTargets);
        
        float lateralVelocity = Vector3.Dot(_velocity, inTransform.right);
        Quaternion tilt = Quaternion.AngleAxis(-lateralVelocity * tiltAmount, Vector3.forward);
        Quaternion newRotation = tilt * Quaternion.LookRotation(lookPosition - inTransform.localPosition);
        return Quaternion.RotateTowards(inTransform.rotation, newRotation, rotateSpeed);
    }
    public override Vector3 GetPosition(Vector3 inPosition)
    {
        Vector3 followTarget = AggregateTargets(_followTargets);
    
        Vector3 newPosition = followTarget + displacement;
        return Vector3.SmoothDamp(inPosition, newPosition, ref _velocity, smoothTime);
    }
}
