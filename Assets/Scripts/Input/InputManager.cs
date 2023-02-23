using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static System.Action<Vector2> onMousePos;
    public static System.Action<Vector2> onScrollMouse;
    public static System.Action<bool> onRightMouse;
    public static System.Action<bool> onLeftMouse;

    public static System.Action<bool> onToggleInputs;

    [SerializeField] private EventSystem eventSystem;
    public Vector2 mousePos;

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

        inputActions.Player.LeftMouse.started += LeftMouseStarted;
        inputActions.Player.LeftMouse.canceled += LeftMouseStarted;
    }

    private void OnEnable()
    {
        inputActions.Player.MousePos.performed += MousePosPerformed;

        inputActions.Player.RightMouse.started += RightMouseStarted;
        inputActions.Player.RightMouse.canceled += RightMouseStarted;

        inputActions.Player.LeftMouse.started += LeftMouseStarted;
        inputActions.Player.LeftMouse.canceled += LeftMouseStarted;
    }

    private void OnDisable()
    {
        inputActions.Player.MousePos.performed -= MousePosPerformed;

        inputActions.Player.RightMouse.started -= RightMouseStarted;
        inputActions.Player.RightMouse.canceled -= RightMouseStarted;

        inputActions.Player.LeftMouse.started -= LeftMouseStarted;
        inputActions.Player.LeftMouse.canceled -= LeftMouseStarted;
    }

    private void MousePosPerformed(InputAction.CallbackContext context)
    {
        onMousePos?.Invoke(context.ReadValue<Vector2>());
        mousePos = context.ReadValue<Vector2>();
    }

    private void RightMouseStarted(InputAction.CallbackContext context)
    {
        onRightMouse?.Invoke(context.started);
    }

    private void LeftMouseStarted(InputAction.CallbackContext context)
    {
        onLeftMouse?.Invoke(context.started);
    }
}
