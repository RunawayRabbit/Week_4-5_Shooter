using UnityEngine;

public class Target : MonoBehaviour
{
    private Arena _arena;
    [SerializeField] private float maxSpeed = 2.2f;
    [SerializeField] private float accelerationRate = 6.0f;
    private float boundaryBufferDistance = 2.0f;

    private Vector3 _velocity;

    private void Awake()
    {
        _arena = GetComponentInParent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} has no Arena in it's parent..");
    }

    private void Update()
    {
        // Get input, work out what the player is asking for.
        Vector2 moveInput = _arena.playerInput.Flying.Move.ReadValue<Vector2>().normalized;
        Vector2 desired2DVelocity = moveInput * maxSpeed;

        // If they are asking to steer into a wall, scale desiredVelocity.
        float distanceToBounds = _arena.ForwardDistanceToBounds(transform.localPosition, desired2DVelocity);
        if (distanceToBounds < boundaryBufferDistance)
            desired2DVelocity *= Mathf.Clamp01(distanceToBounds / boundaryBufferDistance);

        // Convert their 2D input into the correct 3D desired velocity.
        Vector3 desired3DVelocity = _arena.Convert2Dto3D(desired2DVelocity);

        // Apply acceleration to _velocity based on the desired velocity.
        float maxAccelForFrame = Time.deltaTime * accelerationRate;
        _velocity = Vector3.MoveTowards(_velocity, desired3DVelocity, maxAccelForFrame);

        // Clamp our movement to stay within the arena bounds.
        Vector3 newPosition = _arena.ConstrainPointToArena(transform.localPosition + _velocity * Time.deltaTime);
        
        // Perform the move.
        transform.localPosition = newPosition;
    }
}
