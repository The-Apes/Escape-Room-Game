using Objects;
using UnityEngine;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactDistance = 3f;

        private Interactable _lastInteractable;
        private ObjectInteractor _objectInteractor;

        private void Awake()
        {
            if (playerCamera == null)
                playerCamera = Camera.main;
            _objectInteractor = FindFirstObjectByType<ObjectInteractor>();
        
        }

        void Update()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
                if (interactable)
                {
                    if (_lastInteractable != interactable)
                    {
                        if (_lastInteractable)
                            _lastInteractable.Highlight(false);
                        if(_objectInteractor.Inspecting) return;
                        interactable.Highlight(true);
                        _lastInteractable = interactable;
                    }
                }
                else if (_lastInteractable)
                {
                    _lastInteractable.Highlight(false);
                    _lastInteractable = null;
                }
            }
            else if (_lastInteractable)
            {
                _lastInteractable.Highlight(false);
                _lastInteractable = null;
            }
        }
    }
}