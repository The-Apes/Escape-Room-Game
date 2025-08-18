using UnityEngine;

namespace Objects
{
    public class KeypadInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private DoorController door;

        public void OnInteract(GameObject heldObject)
        {
            if (door != null)
            {
                door.ToggleDoor();
            }
        }
    }
}
