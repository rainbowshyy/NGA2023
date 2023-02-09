using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static System.Action<Vector2> onMousePos;
    public static System.Action<Vector2> onScrollMouse;
    public static System.Action<bool> onRightMouse;

    public PlayerInputAsset inputActions { get; private set; }

    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        inputActions = new PlayerInputAsset();
        inputActions.Player.Enable();

        inputActions.Player.MousePos.performed += MousePosPerformed;

        inputActions.Player.RightMouse.started += RightMouseStarted;
        inputActions.Player.RightMouse.canceled += RightMouseStarted;
    }

    private void MousePosPerformed(InputAction.CallbackContext context)
    {
        onMousePos?.Invoke(context.ReadValue<Vector2>());
    }

    private void RightMouseStarted(InputAction.CallbackContext context)
    {
        onRightMouse?.Invoke(context.started);
    }
}
