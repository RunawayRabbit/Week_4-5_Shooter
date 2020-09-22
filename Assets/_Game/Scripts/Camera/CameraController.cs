using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerShip = default;
    [SerializeField] private GameObject targetReticle = default;
    [SerializeField] private GameObject arena = default;
    private ICameraBehaviour _camBehaviour;

    [SerializeField] private CamAttributes overShoulderAttribs = default;
    [SerializeField] private CamAttributes topDownAttribs = default;
    
    private Arena _arena;
    private Vector3 _velocity;

    private void Awake()
    {
        _arena = arena.GetComponent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} can't find the Arena! Did you forget to set a reference to it?");
        _arena.OnModeChange += ArenaOnOnModeChange;
        
        Debug.Assert(playerShip, $"{gameObject.name} has no reference to the player ship!");
        Debug.Assert(targetReticle, $"{gameObject.name} has no reference to the target reticle!");

        Debug.Assert(overShoulderAttribs, $"{gameObject.name} doesn't have any assigned Over Shoulder camera attribs!");
        overShoulderAttribs.followTargets = new List<GameObject> { playerShip };
        overShoulderAttribs.lookTargets = new List<GameObject> { targetReticle };

        Debug.Assert(topDownAttribs, $"{gameObject.name} doesn't have any assigned Top Down camera attribs!!");
        topDownAttribs.followTargets = new List<GameObject> {  targetReticle, playerShip, arena };
        topDownAttribs.lookTargets = new List<GameObject> {  targetReticle, arena };

        _camBehaviour = new FollowCam(ref topDownAttribs);
    }

    private void ArenaOnOnModeChange(Arena.Mode mode)
    {
        if (mode == Arena.Mode.Horizontal)
            _camBehaviour.Attribs = topDownAttribs;
        else
            _camBehaviour.Attribs = overShoulderAttribs;
    }


    private void LateUpdate()
    {
        transform.position = _camBehaviour.GetPosition(transform.position);
        transform.rotation = _camBehaviour.GetRotation(transform);
        Camera.main.fieldOfView = _camBehaviour.GetFoV(Camera.main.fieldOfView);
    }
}
