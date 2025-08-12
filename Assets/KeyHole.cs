using UnityEngine;
using UnityEngine.Serialization;

public class KeyHole : MonoBehaviour, IInteractable
{
    public GameObject key;
    
    [SerializeField] private Transform slot;

    public void OnInteract(GameObject HeldObject)
    {
        print("KeyHole Interacted with");
        FindFirstObjectByType<PickUpScript>().placeObject(slot);
    }
}
