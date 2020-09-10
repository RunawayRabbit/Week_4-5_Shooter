using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject targetReticle;
    [SerializeField] private GameObject arena;
    [SerializeField] private Vector3 cameraDisplacement;
    
    private Arena _arena;
    
    private void Awake()
    {
        Debug.Assert(playerShip, $"{gameObject.name} has no reference to the player ship!");
        Debug.Assert(targetReticle, $"{gameObject.name} has no reference to the target reticle!");

        _arena = arena.GetComponent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} can't find the Arena! Did you forget to set a reference to it?");
    }

    private void Update()
    {
        transform.LookAt(targetReticle.transform);
        transform.position = playerShip.transform.position + cameraDisplacement;

        //Move towards that transform.

    }
}
