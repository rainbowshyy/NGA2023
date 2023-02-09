using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static System.Action<Vector2> onMouseDelta;

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

        inputActions.Player.MouseDelta.performed += MouseDeltaPerformed;
        inputActions.Player.MouseDelta.canceled += MouseDeltaPerformed;
    }

    private void MouseDeltaPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onMouseDelta?.Invoke(context.ReadValue<Vector2>());
        }
        else
        {
            onMouseDelta?.Invoke(Vector2.zero);
        }
    }
}
