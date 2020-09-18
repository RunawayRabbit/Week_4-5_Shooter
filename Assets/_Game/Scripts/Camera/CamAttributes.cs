using UnityEngine;


[CreateAssetMenu(fileName = "CamAttributes", menuName = "Scriptables/Camera Behaviour attributes", order = 0)]
public class CamAttributes : ScriptableObject
{
    public Vector3 displacement = default;
    public float rotateSpeed = 3.0f;
    public float smoothTime = 0.1f;
    public float tiltAmount = 3.0f;
    public float FoV = 40.0f;
    public float FoVMaxChangeRate = 5.0f;
}
