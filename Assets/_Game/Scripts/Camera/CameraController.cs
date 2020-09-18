using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject targetReticle;
    [SerializeField] private GameObject arena;
    private ICameraBehaviour _camBehaviour;

    [SerializeField] private CamAttributes OverShoulderAttribs;
    [SerializeField] private CamAttributes TopDownAttribs;
    
    private Arena _arena;
    private Vector3 _velocity;

    private void Awake()
    {
        Debug.Assert(playerShip, $"{gameObject.name} has no reference to the player ship!");
        Debug.Assert(targetReticle, $"{gameObject.name} has no reference to the target reticle!");

        _arena = arena.GetComponent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} can't find the Arena! Did you forget to set a reference to it?");
        
        Debug.Assert(OverShoulderAttribs, $"{gameObject.name} doesn't have any assigned Over Shoulder camera attribs!");
        Debug.Assert(TopDownAttribs, $"{gameObject.name} doesn't have any assigned Top Down camera attribs!!");
        
        _camBehaviour = new OverShoulderCam(ref OverShoulderAttribs, playerShip, targetReticle);
    }

    
    
    private void LateUpdate()
    {
        transform.localPosition = _camBehaviour.GetPosition(transform.localPosition);
        transform.rotation = _camBehaviour.GetRotation(transform);
        Camera.main.fieldOfView = _camBehaviour.GetFoV(Camera.main.fieldOfView);
    }
}
