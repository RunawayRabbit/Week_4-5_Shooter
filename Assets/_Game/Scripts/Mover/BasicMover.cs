using UnityEngine;

public class BasicMover : MonoBehaviour
{
    private IMover _mover;

    private void Awake()
    {
        _mover = _mover = GetComponent<IMover>();
    }

    private void Update()
    {
        _mover?.Move();
    }
}