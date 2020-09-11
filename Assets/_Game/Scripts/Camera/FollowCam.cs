using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject targetReticle;
    [SerializeField] private GameObject arena;
    private CameraBehaviour _camBehaviour;

    [SerializeField] private CamBehaviourData[] camData;
    
    private Arena _arena;
    private Vector3 _velocity;

    private void Awake()
    {
        Debug.Assert(playerShip, $"{gameObject.name} has no reference to the player ship!");
        Debug.Assert(targetReticle, $"{gameObject.name} has no reference to the target reticle!");

        _arena = arena.GetComponent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} can't find the Arena! Did you forget to set a reference to it?");
        
        Debug.Assert(camData.Length > 0, $"{gameObject.name} doesn't have any assigned camera data!");
        
        // I think this isn't working because it's taking a copy and not a ref?
        _camBehaviour = new OverShoulderCam(camData[0], playerShip, targetReticle);
    }

    private void Update()
    {
        transform.localPosition = _camBehaviour.GetPosition(transform.localPosition);
        transform.rotation = _camBehaviour.GetRotation(transform);
    }
}
