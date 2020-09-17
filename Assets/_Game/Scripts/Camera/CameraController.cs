using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerShip = default;
    [SerializeField] private GameObject targetReticle = default;
    [SerializeField] private GameObject arena = default;
    
    public ICameraBehaviour _camBehaviour;

    public CameraAttributes OverShoulderAttribs;
    public CameraAttributes TopDownAttribs;

    private Vector3 _lookTarget;

    private Arena _arena;
    private Camera _camera;
    
    private void Awake()
    {
        _camera = Camera.main;
        Debug.Assert(playerShip, $"{gameObject.name} has no reference to the player ship!");
        Debug.Assert(targetReticle, $"{gameObject.name} has no reference to the target reticle!");

        _arena = arena.GetComponent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} can't find the Arena! Did you forget to set a reference to it?");

        _camBehaviour = DefaultTopDownCam();

        _arena.OnModeChange += ChangeMode;
    }

    private void ChangeMode(Arena.Mode newMode)
    {
        var oldCam = _camBehaviour;
        ICameraBehaviour newCam = newMode == Arena.Mode.Horizontal ? DefaultTopDownCam() : DefaultOverShoulderCam();
        _camBehaviour = new OverShoulderCam(ref OverShoulderAttribs, playerShip, targetReticle);//new TransitionCam(oldCam, newCam, 3.0f);
    }
    
    private ICameraBehaviour DefaultTopDownCam()
    {
        var followTargets = new List<GameObject> {playerShip, _arena.gameObject};
        return new TopDownCam(ref TopDownAttribs, followTargets, playerShip);
    }

    private ICameraBehaviour DefaultOverShoulderCam()
    {
        return new OverShoulderCam(ref OverShoulderAttribs, playerShip, targetReticle);
    }

    private void Update()
    {
        var trans = transform;
        var pos = trans.position;
        var fov = _camera.fieldOfView;

        _camBehaviour.MoveCamera(this, ref pos, ref _lookTarget, out Vector3 cameraUp, ref fov);
        
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(_lookTarget, cameraUp);
        _camera.fieldOfView = fov;
    }
}
