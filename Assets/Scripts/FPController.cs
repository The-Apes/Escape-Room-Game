using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 3f; //will we have jumping? maybe remove jumping for immersion
    public float gravity = -9.81f;
    [NonSerialized] public bool canMove = true; //if we want to disable movement, for example when you inspecting an object
    
    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f; //do we need?
    [NonSerialized] public bool canLook = true; //if we want to disable looking around, for example when you inspecting an object
    
    private CharacterController _characterController;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private bool isGrounded;
    private float verticalRotation = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        //put this in a manager class maybe?
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
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
    }

        