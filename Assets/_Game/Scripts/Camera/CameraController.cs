using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerShip = default;
    [SerializeField] private GameObject targetReticle = default;
    [SerializeField] private GameObject arena = default;
    
    public ICameraBehaviour _camBehaviour;

    public CameraAttributes OverShoulderAttribs;
    public CameraAttributes TopDownAttribs;
    
    private Arena _arena;
    private Vector3 _velocity;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        Debug.Assert(playerShip, $"{gameObject.name} has no reference to the player ship!");
        Debug.Assert(targetReticle, $"{gameObject.name} has no reference to the target reticle!");

        _arena = arena.GetComponent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} can't find the Arena! Did you forget to set a reference to it?");
        
        _camBehaviour = new TopDownCam(TopDownAttribs, playerShip, targetReticle);

        _arena.OnModeChange += ChangeMode;
    }

    private void ChangeMode(Arena.Mode newMode)
    {
        var oldCam = _camBehaviour;
        ICameraBehaviour newCam;
        if (newMode == Arena.Mode.Horizontal)
            newCam = new TopDownCam(TopDownAttribs,playerShip, targetReticle);
        else
            newCam = new OverShoulderCam(OverShoulderAttribs, playerShip, targetReticle);
        
        _camBehaviour = new TransitionCam(oldCam,newCam, 0.8f);
    }

    private void Update()
    {
        transform.localPosition = _camBehaviour.GetPosition(this);
        transform.rotation = _camBehaviour.GetRotation(this);
        _camera.fieldOfView = _camBehaviour.GetFoV(this, _camera.fieldOfView);
    }
}
