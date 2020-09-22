using UnityEngine;

[SelectionBase]
public class ZoomyBoy : Enemy
{
    [SerializeField] private RammingMover rammingMover = default;

    protected override void Awake()
    {
        rammingMover = GetComponent<RammingMover>();
        base.Awake();
    }

    protected void Update()
    {
        //   ¯\_(ツ)_/¯
        rammingMover.Move();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player")) Debug.Log("Got'm!");
    }
}