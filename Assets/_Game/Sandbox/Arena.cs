using UnityEngine;

public class Arena : MonoBehaviour
{
    public enum Mode
    {
        Horizontal,
        Vertical
    }
    
    public GameplayControls playerInput;

    public Vector2 horizontalArena;
    public Vector2 vertcalArena;

    public Mode currentMode;
    private void Awake()
    {
        currentMode = Mode.Horizontal;
        playerInput = new GameplayControls();
    }
    
    private void OnEnable()
    {
        playerInput.Enable();
    }
    
    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        if (playerInput.Flying.DEBUG.triggered)
        {
            currentMode = currentMode == Arena.Mode.Horizontal ? Arena.Mode.Vertical : Arena.Mode.Horizontal; 
            Debug.Log($"MODE SWITCHED: {currentMode}");
        }
    }
}
