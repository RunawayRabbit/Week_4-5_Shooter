using UnityEngine;

public interface ICameraBehaviour
{
    void MoveCamera(CameraController cam, ref Vector3 position, ref Vector3 lookAt, out Vector3 cameraUp, ref float fov);
}