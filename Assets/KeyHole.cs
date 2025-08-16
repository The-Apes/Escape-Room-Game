using Managers;
using UnityEngine;
using UnityEngine.Serialization;

public class KeyHole : MonoBehaviour, IInteractable
{
    public GameObject key;
    
    [SerializeField] private Transform slot;

    public void OnInteract(GameObject HeldObject)
    {
        print("KeyHole Interacted with");
        if(!HeldObject) return;
        print("HeldObject: " + HeldObject);
        print("key: " + key);
        if(!HeldObject.Equals(key)) return;
        print("Key matches, placing object in slot");
        FindFirstObjectByType<PickUpScript>().PlaceObject(slot);
        HeldObject.tag = "Untagged"; //remove tag so we don't pick it up again
        PuzzleManager.instance.SetPuzzleStagee(1);
    }
}
