using UnityEngine;

public class Target : MonoBehaviour
{
    private Arena _arena;
    [SerializeField] private float maxSpeed = 2.2f;
    [SerializeField] private float accelerationRate = 6.0f;

    private Vector2 _velocity;

    private void Awake()
    {
        _arena = GetComponentInParent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} has no Arena in it's parent..");
    }

    private void Update()
    {
        var moveInput = _arena.playerInput.Flying.Move.ReadValue<Vector2>();

        var desiredVelocity = new Vector2(moveInput.x, moveInput.y) * maxSpeed;
        var maxAccelForFrame = (Time.deltaTime * accelerationRate);

        var newVelocity = Vector2.MoveTowards(_velocity, desiredVelocity, maxAccelForFrame);
        _velocity = newVelocity;
        Debug.Log($"{moveInput.magnitude}, {newVelocity.magnitude}");

        Vector3 positionDelta;
        if (_arena.currentMode == Arena.Mode.Horizontal)
        {
            positionDelta = new Vector3(newVelocity.x, 0.0f, newVelocity.y); 
        }
        else
        {
            positionDelta = new Vector3(newVelocity.x, newVelocity.y,0.0f); 
        }        
        
        transform.position += positionDelta * (Time.deltaTime * maxSpeed);



        /* Because I was listening to a podcast while I wrote this code....
         * 
         * ***                PARADOX PILLARS                *** *
         * 
         * LOOKS GOOD PLAYS PERFECT *
         * 
         * Paradox games never sacrifice function to form,
         * and always prioritizes gameplay over everything else.
         *
         * 
         * GIVES AGENCY TO THE PLAYER *
         *
         * Paradox games give players the tools to express their creativity,
         * make their gaming experience their own. From character customization to
         * emergent storytelling or extensive modding.
         *
         *
         * 
         */

    }
}
