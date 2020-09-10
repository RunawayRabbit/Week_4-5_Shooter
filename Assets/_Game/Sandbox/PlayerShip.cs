using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
    [SerializeField] GameObject target = default;
    private float _distanceFromTarget = 3.0f;
    private Arena _arena;
    private Rigidbody rigidbody;

    private void Awake()
    {
        if(!target) Debug.Assert(target, $"{gameObject.name} does not have a target object set!");
        
        _arena = GetComponentInParent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} has no Arena in it's parent..");

        rigidbody = GetComponent<Rigidbody>();
        Debug.Assert(rigidbody, $"{gameObject.name} has no Rigidbody! It should have one!");
    }

    private void Update()
    {
        var targetTransform = target.transform;

        if (_arena.CurrentMode == Arena.Mode.Horizontal)
        {
            transform.localPosition = target.transform.localPosition - Vector3.back * _distanceFromTarget;
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            Quaternion look = Quaternion.LookRotation(
                targetTransform.localPosition - transform.localPosition, Vector3.up);
            transform.localRotation = look;

        }
    }
}
