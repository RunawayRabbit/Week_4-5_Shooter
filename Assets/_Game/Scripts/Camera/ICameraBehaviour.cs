
using UnityEngine;

public interface ICameraBehaviour
{
    Vector3 GetPosition(Vector3 inPosition);
    Quaternion GetRotation(Transform transform);
    float GetFoV(float currentFoV);
}
