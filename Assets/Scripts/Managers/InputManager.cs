using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.PlayerActions player;

    private PlayerMotor Motor;
    private PlayerLook Look;

    void Awake()
    {
        playerInput = new PlayerInput();
        player = playerInput.Player;
        Motor = GetComponent<PlayerMotor>();
        if (Motor == null)
            Debug.LogError("PlayerMotor component not found on this GameObject!");
        Look = GetComponent<PlayerLook>();
        if (Look == null)
            Debug.LogError("PlayerLook component not found on this GameObject!");
        player.Jump.performed += OnJump;
    }

    void FixedUpdate()
    {
        if (Motor != null)
            Motor.ProcessMove(player.Movement.ReadValue<Vector2>());
    }
    void LateUpdate()
    {
        if (Look != null)
            Look.ProcessLook(player.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        playerInput.Enable();
        player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
        player.Disable();
        player.Jump.performed -= OnJump;
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (Motor != null)
            Motor.Jump();
    }
}
