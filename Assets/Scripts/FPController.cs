using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class FPController : MonoBehaviour
{
    [Header("Body Settings")] 
    public float Radius = 0.3f;
    
    [Header("Movement Settings")] 
    public float standingSpeed = 5f;
    public float crouchingSpeed = 1.5f;
    public float proneSpeed = 0.6f;

    public float jumpForce = 3f; //will we have jumping? maybe remove jumping for immersion
    public float gravity = -9.81f;

    [NonSerialized]
    public bool canMove = true; //if we want to disable movement, for example when you inspecting an object

    [Header("Look Settings")] public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;
    public float standingEyeHeight = 1.48f; //do we need?
    public float crouchingEyeHeight = 0.92f;
    public float proneEyeHeight = 0.7f;

    [NonSerialized]
    public bool canLook = true; //if we want to disable looking around, for example when you inspecting an object

    [Header("Crouch Settings")]
    public float standHeight = 1.52f; //height when standing
    public float crouchHeight = 0.5f;
    public float proneHeight = 0.1f; //height when prone, not sure if we will use this//height when crouching

    private CharacterController _characterController;
    private float moveSpeed;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool crouching;
    private float verticalRotation = 0f;
    
    enum height
    {
        Standing,
        Crouching,
        Prone
    }
    
    private height currentHeight = height.Standing;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        //put this in a manager class maybe?
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        moveSpeed = standingSpeed;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void HandleMovement()
    {
        if (!canMove) return;

        moveSpeed = currentHeight switch
        {
            height.Standing => standingSpeed,
            height.Crouching => crouchingSpeed,
            height.Prone => proneSpeed,
            _ => moveSpeed
        };
        
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        _characterController.Move(move * moveSpeed * Time.deltaTime);
        if (_characterController.isGrounded && velocity.y < 0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);
    }

    public void HandleLook()
    {
        if (!canLook) return;
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -3f * gravity);
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        // I read about the is operator on GeeksforGeeks, and figured about interaction types by reading the Unity documentation
        // not sure if it warrants a reference.
        if (!context.performed) return; //only fires when either a tap or hold interaction is registered, not on button press
        if (context.interaction is TapInteraction)
        {
            if (currentHeight == height.Standing)
            {
                _characterController.height = crouchHeight;
                cameraTransform.localPosition = new Vector3(0, crouchingEyeHeight, 0);

                currentHeight = height.Crouching;
            }
            else
            {
                _characterController.height = standHeight;
                cameraTransform.localPosition = new Vector3(0, standingEyeHeight, 0);

                currentHeight = height.Standing;
            }
        }
        else if (context.interaction is HoldInteraction)
        {
            _characterController.height = proneHeight;
            cameraTransform.localPosition = new Vector3(0, proneEyeHeight, 0);

            currentHeight = height.Prone;
        }
    }
    
}

        