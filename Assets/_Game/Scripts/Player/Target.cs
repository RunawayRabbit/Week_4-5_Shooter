using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour
{
    private Arena _arena;
    [SerializeField] private float maxSpeed = 2.2f;
    [SerializeField] private float accelerationRate = 6.0f;
    private float boundaryBufferDistance = 2.0f;

    private Renderer _renderer;
    
    private Vector3 _velocity;
    private Vector2 _moveInput;
    
    private void Start()
    {
        _arena = Arena.Instance;
        _arena.OnModeChange += OnModeChange;
        _renderer = GetComponent<Renderer>();
        if(!GetComponent<Renderer>()) Debug.Log($"{gameObject.name} has no renderer component, it should probably have one.");

        _renderer.enabled = false;
    }

    private void OnModeChange(Arena.Mode newMode)
    {
        if (newMode == Arena.Mode.Horizontal)
            _renderer.enabled = false;
        else
            _renderer.enabled = true;
    }

    // ReSharper disable once UnusedMember.Global
    public void OnInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        Vector2 desired2DVelocity = _moveInput * maxSpeed;

        /* @TODO: I don't like this solution so much. What we really want is a Vector2 so we can dampen our
         velocity component-wise. This approach causes us to "stick" on invisible edge walls.. */
        float distanceToBounds = _arena.ForwardDistanceToBounds(transform.localPosition, desired2DVelocity);
        if (distanceToBounds < boundaryBufferDistance)
            desired2DVelocity *= Mathf.Clamp01(distanceToBounds / boundaryBufferDistance);

        Vector3 desired3DVelocity = _arena.Convert2Dto3D(desired2DVelocity, transform.localPosition);
        
        float maxAccelForFrame = Time.deltaTime * accelerationRate;
        _velocity = Vector3.MoveTowards(_velocity, desired3DVelocity, maxAccelForFrame);
 
        Vector3 slidingArena = transform.localPosition - (_arena.velocity.x * Vector3.right);
        Vector3 newPosition = _arena.ConstrainToBounds(slidingArena + _velocity * Time.deltaTime);

        transform.localPosition = newPosition;
    }
}
