using UnityEditor;
using UnityEngine;

public class RammingMover : MonoBehaviour, IMover
{
    private float _disengageRangeSq;
    private float _freezeAndChargeRangeSq;

    private IMover _patrolMover;

    [SerializeField] private float chargeSpeed = 8.0f;
    private State currentState = State.Patrolling;
    [SerializeField] private float disengageRange = 25.0f;
    [SerializeField] private float freezeAndChargeRange = 10.0f;
    [SerializeField] private float maxTurnSpeed = 2.0f;
    [SerializeField] private Transform player = default;
    private float timeInThisState;
    [SerializeField] private float timeToCharge = 1.4f;
    [SerializeField] private float timeToFreeze = 2.8f;

    public Vector3 Move()
    {
        var delta = Vector3.zero;
        var facingVectorToTarget = player.transform.position - transform.position;

        switch (currentState)
        {
            case State.Patrolling:
                delta = _patrolMover.Move();
                transform.rotation = Quaternion.LookRotation(delta, Vector3.up);

                if (facingVectorToTarget.sqrMagnitude < _freezeAndChargeRangeSq)
                {
                    currentState = State.Freezing;
                    timeInThisState = 0.0f;
                }

                break;

            case State.Freezing:
                TurnToFaceTarget(facingVectorToTarget);
                timeInThisState += Time.deltaTime;

                if (timeInThisState >= timeToFreeze)
                {
                    currentState = State.Charging;
                    timeInThisState = 0.0f;
                }

                break;

            case State.Charging:
                TurnToFaceTarget(facingVectorToTarget);
                timeInThisState += Time.deltaTime;

                delta = chargeSpeed * transform.forward;
                transform.position += delta * Time.deltaTime;

                if (facingVectorToTarget.sqrMagnitude >= _disengageRangeSq)
                {
                    currentState = State.Patrolling;
                }
                else if (timeInThisState >= timeToCharge)
                {
                    currentState = State.Freezing;
                    timeInThisState = 0.0f;
                }

                break;
        }

        return delta;
    }

    public void Awake()
    {
        if (!player)
            player = GameObject.FindWithTag("Player").transform;

        var movers = GetComponents<IMover>();
        foreach (var mover in movers)
        {
            if (mover == this) continue;
            _patrolMover = mover;
        }

        if (_patrolMover == null)
            Debug.LogWarning(
                $"{gameObject.name}'s RammingMover needs a second IMover on the object to act as a patrolling behaviour!");

        _freezeAndChargeRangeSq = freezeAndChargeRange * freezeAndChargeRange;
        _disengageRangeSq = disengageRange * disengageRange;
    }

    private void OnDrawGizmosSelected()
    {
        var position = transform.position;

        Handles.color = new Color(1.0f, 0.2f, 0.2f, 0.1f);
        Handles.DrawSolidDisc(position, Vector3.up, freezeAndChargeRange);
        Handles.color = new Color(0.2f, 1.0f, 0.2f, 0.1f);
        Handles.DrawSolidDisc(position, Vector3.up, disengageRange);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _freezeAndChargeRangeSq = freezeAndChargeRange * freezeAndChargeRange;
        _disengageRangeSq = disengageRange * disengageRange;
    }
#endif

    private void TurnToFaceTarget(Vector3 facingVectorToTarget)
    {
        var lookAtTarget = Quaternion.LookRotation(facingVectorToTarget, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtTarget, maxTurnSpeed);
    }

    private enum State
    {
        Patrolling,
        Freezing,
        Charging
    }
}