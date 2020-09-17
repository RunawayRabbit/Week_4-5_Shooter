using UnityEngine;

public interface ICameraBehaviour
{
    Vector3 GetPosition(CameraController cam);
    Quaternion GetRotation(CameraController cam);
    float GetFoV(CameraController cam, float currentFoV);
}