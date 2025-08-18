using Managers;
using UnityEngine;

namespace Objects
{
    public class KeypadInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private DoorController door;
        [SerializeField] private GameObject key;
        public void OnInteract(GameObject heldObject)
        {
            if (door == null) return;
            if(!heldObject) return;
            if(!heldObject.Equals(key)) return;
            door.ToggleDoor();
            PuzzleManager.instance.SetPuzzleStage(3);
            DialogueManager.instance.SayLine("GGs, you beat the prototype! :) come talk to me for more info about this prototype");
        }
    }
}
