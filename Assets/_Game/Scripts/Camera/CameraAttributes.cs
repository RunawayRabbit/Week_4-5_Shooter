using UnityEngine;


[CreateAssetMenu(fileName = "CamBehaviourData", menuName = "Scriptables/Cam Behaviour Data", order = 0)]
public class CameraAttributes : ScriptableObject
{
    public Vector3 displacement = default;
    public float rotateSpeed = 3.0f;
    public float smoothTime = 0.1f;
    public float tiltAmount = 3.0f;
    public float FoV = 40.0f;
    public float FoVSmoothTime = 5.0f;
}
