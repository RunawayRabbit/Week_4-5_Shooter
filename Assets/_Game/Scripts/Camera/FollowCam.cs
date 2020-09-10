using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject targetReticle;
    [SerializeField] private GameObject arena;
    
    [SerializeField] private Vector3 displacement = default;
    [SerializeField] private float rotateSpeed = 3.0f;
    [SerializeField] private float smoothTime = 0.1f;
    
    private Arena _arena;
    private Vector3 _velocity;

    private void Awake()
    {
        Debug.Assert(playerShip, $"{gameObject.name} has no reference to the player ship!");
        Debug.Assert(targetReticle, $"{gameObject.name} has no reference to the target reticle!");

        _arena = arena.GetComponent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} can't find the Arena! Did you forget to set a reference to it?");
    }

    private void Update()
    {
        var currentPosition = transform.position;
        Vector3 newPosition = playerShip.transform.position + displacement;

        newPosition = Vector3.SmoothDamp(currentPosition, newPosition, ref _velocity, smoothTime);
        transform.position = newPosition;
        
        Quaternion newRotation = Quaternion.LookRotation(targetReticle.transform.position - currentPosition);
        Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed);
        transform.rotation = newRotation;
    }
}
