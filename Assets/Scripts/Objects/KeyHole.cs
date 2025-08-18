using Managers;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects
{
    public class KeyHole : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject key;
        [SerializeField] private GameObject door;
        [SerializeField] private Transform slot;

        public void OnInteract(GameObject heldObject)
        {
            print("KeyHole Interacted with");
            if(!heldObject) return;
            print("HeldObject: " + heldObject);
            print("key: " + key);
            if(!heldObject.Equals(key)) return;
            print("Key matches, placing object in slot");
            FindFirstObjectByType<ObjectInteractor>().PlaceObject(slot);
            heldObject.tag = "Untagged"; //remove tag so we don't pick it up again
            PuzzleManager.instance.SetPuzzleStage(2);
            Destroy(door);
        }
    }
}
