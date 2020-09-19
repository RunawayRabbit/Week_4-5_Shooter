using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CamAttributes", menuName = "Scriptables/Camera Behaviour", order = 0)]
public class CamAttributes : ScriptableObject
{
    public Vector3 displacement = default;
    public float rotateSpeed = 3.0f;
    public float smoothTime = 0.1f;
    public float tiltAmount = 3.0f;
    public float fieldOfView = 40.0f;
    public float fieldOfViewSmoothTime = 5.0f;
    public Vector3 cameraUp = Vector3.up;
    
    [HideInInspector] public List<GameObject> followTargets;
    [HideInInspector] public List<GameObject> lookTargets;
}
