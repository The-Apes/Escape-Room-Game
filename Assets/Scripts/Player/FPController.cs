using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Player
{
    public class FPController : MonoBehaviour
    {
        public bool useFootsteps = true;
        public bool UseFootsteps => useFootsteps;
        public float radius = 0.3f;

        [Header("Movement Settings")] 
        public float standingSpeed = 5f;
        public float crouchingSpeed = 1.5f;
        public float proneSpeed = 0.6f;

        public float jumpForce = 3f;
        public float gravity = -9.81f;

        [NonSerialized]
        public bool CanMove = true;
        public bool CanUseHeadbob = true;

        [Header("Headbob Settings")]
        [SerializeField] private float walkBobSpeed = 14f;
        [SerializeField] private float walkBobAmount = 0.05f;
        [SerializeField] private float sprintBobSpeed = 18f;
        [SerializeField] private float sprintBobAmount = 0.1f;
        [SerializeField] private float crouchBobSpeed = 8f;
        [SerializeField] private float crouchBobAmount = 0.025f;
        private float _defaultYPos = 0;
        private float _timer;

        [Header("Footstep Settings")]
        [SerializeField] private float baseStepSpeed = 0.5f;
        [SerializeField] private float crouchStepMultiplier = 1.5f;
        [SerializeField] private float proneStepMultiplier = 2.5f;
        [SerializeField] private float sprintStepMultiplier = 0.6f;
        [SerializeField] private AudioSource footstepAudioSource = default;
        [SerializeField] private AudioClip[] woodClips = default;
        [SerializeField] private AudioClip[] metalClips = default;
        [SerializeField] private AudioClip[] concreteClips = default;
        private float _footstepTimer = 0;
        private float GetCurrentOffset => IsCrouching ? baseStepSpeed * crouchStepMultiplier : IsProne ? baseStepSpeed * proneStepMultiplier : IsSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;

        [Header("Look Settings")]
        public Transform cameraTransform;
        public float lookSensitivity = 2f;
        public float verticalLookLimit = 90f;
        public float standingEyeHeight = 1.48f;
        public float crouchingEyeHeight = 0.92f;
        public float proneEyeHeight = 0.7f;

        [NonSerialized]
        public bool CanLook = true;

        [Header("Crouch Settings")]
        public float standHeight = 1.52f;
        public float crouchHeight = 0.5f;
        public float proneHeight = 0.1f;

        private CharacterController _characterController;
        private float _moveSpeed;
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private Vector3 _velocity;
        private float _verticalRotation;

        private enum Height
        {
            Standing,
            Crouching,
            Prone
        }

        private Height _currentHeight = Height.Standing;
        private bool _isSprinting = false; // Add sprinting logic if needed

        // Properties for state checks
        private bool IsCrouching => _currentHeight == Height.Crouching;
        private bool IsProne => _currentHeight == Height.Prone;
        private bool IsSprinting => _isSprinting;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _defaultYPos = cameraTransform.localPosition.y;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _moveSpeed = standingSpeed;
        }

        private void Update()
        {
            HandleMovement();
            HandleLook();
            HandleHeadbob();
            HandleFootsteps();
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

        private void HandleHeadbob()
        {
            if (!CanUseHeadbob) return;
            if (!_characterController.isGrounded) return;
            if (Mathf.Abs(_moveInput.x) > 0.1f || Mathf.Abs(_moveInput.y) > 0.1f)
            {
                float bobSpeed, bobAmount;
                switch (_currentHeight)
                {
                    case Height.Crouching:
                        bobSpeed = crouchBobSpeed;
                        bobAmount = crouchBobAmount;
                        break;
                    case Height.Prone:
                        // No headbob for prone
                        return;
                    default:
                        bobSpeed = _isSprinting ? sprintBobSpeed : walkBobSpeed;
                        bobAmount = _isSprinting ? sprintBobAmount : walkBobAmount;
                        break;
                }

                _timer += Time.deltaTime * bobSpeed;
                cameraTransform.localPosition = new Vector3(
                    cameraTransform.localPosition.x,
                    _defaultYPos + Mathf.Sin(_timer) * bobAmount,
                    cameraTransform.localPosition.z);
            }
        }

        private void HandleFootsteps()
        {
            if (!useFootsteps || footstepAudioSource == null) return;
            if (!_characterController.isGrounded) return;
            if (_moveInput == Vector2.zero) return;

            _footstepTimer -= Time.deltaTime;

            if (_footstepTimer <= 0)
            {
                if (Physics.Raycast(cameraTransform.position, Vector3.down, out RaycastHit hit, 3))
                {
                    AudioClip[] clipsToUse = woodClips;
                    if (hit.collider.CompareTag("CONCRETE") && concreteClips != null && concreteClips.Length > 0)
                        clipsToUse = concreteClips;
                    else if (hit.collider.CompareTag("METAL") && metalClips != null && metalClips.Length > 0)
                        clipsToUse = metalClips;
                    else if (hit.collider.CompareTag("WOOD") && woodClips != null && woodClips.Length > 0)
                        clipsToUse = woodClips;

                    if (clipsToUse != null && clipsToUse.Length > 0)
                    {
                        footstepAudioSource.PlayOneShot(clipsToUse[UnityEngine.Random.Range(0, clipsToUse.Length)]);
                    }
                }

                _footstepTimer = GetCurrentOffset;
            }
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed && _characterController.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }

        public void Crouch(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
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