
using UnityEngine;

public interface ICameraBehaviour
{
    CamAttributes Attribs { get; set; }
    Vector3 GetPosition(Vector3 inPosition);
    Quaternion GetRotation(Transform transform);
    float GetFoV(float currentFoV);
}
