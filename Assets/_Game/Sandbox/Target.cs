using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour
{
    private Arena _arena;
    [SerializeField] private float maxSpeed = 2.2f;
    [SerializeField] private float accelerationRate = 6.0f;
    private float boundaryBufferDistance = 2.0f;

    private Vector3 _velocity;
    private Vector2 _moveInput;
    
    private void Awake()
    {
        _arena = GetComponentInParent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} has no Arena in it's parent..");
    }

    // ReSharper disable once UnusedMember.Global
    public void OnInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        // Get input, work out what the player is asking for.
        Vector2 desired2DVelocity = _moveInput * maxSpeed;

        /* @TODO: I don't like this solution so much. What we really want is a Vector2 so we can dampen our
         velocity component-wise. This approach causes us to "stick" on invisible edge walls.. */
        // If they are asking to steer into a wall, scale desiredVelocity.
        float distanceToBounds = _arena.ForwardDistanceToBounds(transform.localPosition, desired2DVelocity);
        if (distanceToBounds < boundaryBufferDistance)
            desired2DVelocity *= Mathf.Clamp01(distanceToBounds / boundaryBufferDistance);

        // Convert 2D input into the correct 3D desired velocity.
        Vector3 desired3DVelocity = _arena.Convert2Dto3D(desired2DVelocity, transform.localPosition);
        
        // Apply acceleration to _velocity based on the desired velocity.
        float maxAccelForFrame = Time.deltaTime * accelerationRate;
        _velocity = Vector3.MoveTowards(_velocity, desired3DVelocity, maxAccelForFrame);
 
        // Constrain final position to the appropriate bounds.
        Vector3 newPosition = _arena.ConstrainToBounds(transform.localPosition + _velocity * Time.deltaTime);
        
        // Perform the move.
        transform.localPosition = newPosition;
    }
}
