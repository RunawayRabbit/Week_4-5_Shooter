using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShields : MonoBehaviour
{
    private bool _isShieldOnCooldown;
    [SerializeField] private GameObject shield;
    [SerializeField] private float shieldCooldown = 20.0f;
    [SerializeField] private float shieldUptime = 5.0f;
    public bool AreShieldsUp { get; private set; }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Shield(InputAction.CallbackContext context)
    {
        if (!_isShieldOnCooldown)
        {
            Debug.Log("Shields go up");
            if (context.phase == InputActionPhase.Started)
            {
                shield.SetActive(true);
                AreShieldsUp = true;
                _isShieldOnCooldown = true;
                StartCoroutine(RaiseShields());
                StartCoroutine(ShieldCooldown());
            }
        }
    }

    private IEnumerator ShieldCooldown()
    {
        yield return new WaitForSeconds(shieldCooldown);
        Debug.Log("Shields ready to go!");
        _isShieldOnCooldown = false;
    }

    private IEnumerator RaiseShields()
    {
        yield return new WaitForSeconds(shieldUptime);
        shield.SetActive(false);
        AreShieldsUp = false;
    }
}