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
        if(!HeldObject.name.Equals(key.name)) return;
        FindFirstObjectByType<PickUpScript>().placeObject(slot);
    }
}
