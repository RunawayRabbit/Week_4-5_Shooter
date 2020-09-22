using UnityEngine;

public class PowerUpTumbler : MonoBehaviour
{
    [SerializeField] private float tumblingSpeed = 10.0f;

    private void Update()
    {
        Quaternion tumbler = Quaternion.Euler(tumblingSpeed * Time.deltaTime,
            tumblingSpeed * 0.5f * Time.deltaTime,
            -tumblingSpeed * Time.deltaTime);

        transform.rotation = tumbler * transform.rotation;
    }
}
