using Player;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;

    private Interactable lastInteractable;
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
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (lastInteractable != interactable)
                {
                    if (lastInteractable != null)
                        lastInteractable.Highlight(false);
                    if(_objectInteractor.inspecting) return;
                    interactable.Highlight(true);
                    lastInteractable = interactable;
                }
            }
            else if (lastInteractable != null)
            {
                lastInteractable.Highlight(false);
                lastInteractable = null;
            }
        }
        else if (lastInteractable != null)
        {
            lastInteractable.Highlight(false);
            lastInteractable = null;
        }
    }
}