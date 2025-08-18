using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Player
{
    // ReSharper disable once InconsistentNaming
    public class FPController : MonoBehaviour
    {
        public float radius = 0.3f;
    
        [Header("Movement Settings")] 
        public float standingSpeed = 5f;
        public float crouchingSpeed = 1.5f;
        public float proneSpeed = 0.6f;

        public float jumpForce = 3f; //will we have jumping? maybe remove jumping for immersion
        public float gravity = -9.81f;

        [NonSerialized]
        public bool CanMove = true; //if we want to disable movement, for example when you're inspecting an object

        [Header("Look Settings")] public Transform cameraTransform;
        public float lookSensitivity = 2f;
        public float verticalLookLimit = 90f;
        public float standingEyeHeight = 1.48f; //do we need?
        public float crouchingEyeHeight = 0.92f;
        public float proneEyeHeight = 0.7f;

        [NonSerialized]
        public bool CanLook = true; //if we want to disable looking around, for example when you're inspecting an object

        [Header("Crouch Settings")]
        public float standHeight = 1.52f; //height when standing
        public float crouchHeight = 0.5f;
        public float proneHeight = 0.1f; //height when prone, not sure if we will use this//height when crouching

        private CharacterController _characterController;
        private float _moveSpeed;
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private Vector3 _velocity;
        private bool _isGrounded;
        private bool _crouching;
        private float _verticalRotation;

        private enum Height
        {
            Standing,
            Crouching,
            Prone
        }
    
        private Height _currentHeight = Height.Standing;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            //put this in a manager class maybe?
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _moveSpeed = standingSpeed;
        }

        private void Update()
        {
            HandleMovement();
            HandleLook();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }

        private void HandleMovement()
        {
            if (!CanMove) return;

            _moveSpeed = _currentHeight switch
            {
                Height.Standing => standingSpeed,
                Height.Crouching => crouchingSpeed,
                Height.Prone => proneSpeed,
                _ => _moveSpeed
            };
        
            Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
            _characterController.Move(move * (_moveSpeed * Time.deltaTime));
            if (_characterController.isGrounded && _velocity.y < 0) _velocity.y = -2f;
            _velocity.y += gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void HandleLook()
        {
            if (!CanLook) return;
            float mouseX = _lookInput.x * lookSensitivity;
            float mouseY = _lookInput.y * lookSensitivity;
            _verticalRotation -= mouseY;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -verticalLookLimit, verticalLookLimit);
            cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (_characterController.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpForce * -3f * gravity);
            }
        }

        public void Crouch(InputAction.CallbackContext context)
        {
            // I read about the is operator on GeeksforGeeks, and figured about interaction types by reading the Unity documentation
            // not sure if it warrants a reference.
            if (!context.performed) return; //only fires when either a tap or hold interaction is registered, not on button press
            if (context.interaction is TapInteraction)
            {
                if (_currentHeight == Height.Standing)
                {
                    _characterController.height = crouchHeight;
                    cameraTransform.localPosition = new Vector3(0, crouchingEyeHeight, 0);

                    _currentHeight = Height.Crouching;
                }
                else
                {
                    _characterController.height = standHeight;
                    cameraTransform.localPosition = new Vector3(0, standingEyeHeight, 0);

                    _currentHeight = Height.Standing;
                }
            }
            else if (context.interaction is HoldInteraction)
            {
                _characterController.height = proneHeight;
                cameraTransform.localPosition = new Vector3(0, proneEyeHeight, 0);

                _currentHeight = Height.Prone;
            }
        }
    
    }
}

        