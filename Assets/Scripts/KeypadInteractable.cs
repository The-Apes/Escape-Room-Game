using UnityEngine;

public class KeypadInteractable : MonoBehaviour, IInteractable
{
    [Header("Keypad Settings")]
    [SerializeField] private DoorController door;

    public void OnInteract(GameObject HeldObject)
    {
        Debug.Log($"{name}: Keypad pressed!");
        if (door != null)
        {
            door.OpenDoor();
        }
    }
}