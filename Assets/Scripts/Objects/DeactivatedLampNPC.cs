using Managers;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Objects
{
    public class DeactivatedLampNpc : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject batteryPrefab;
        [SerializeField] private Transform slot;
        [SerializeField] private AudioClip[] insertBatterySound;
        private int _batteryCount;

        public void OnInteract(GameObject heldObject)
        {
            Debug.Log("Deactivated Lamp NPC Interacted with");
            if(!heldObject) return;
            Debug.Log("HeldObject: " + heldObject);
            Debug.Log("batteryPrefab: " + batteryPrefab);
            if(!heldObject.name.Contains("Lamp Battery")) return;
            Debug.Log("is a battery, placing object in Lamp");
            
            AudioSource.PlayClipAtPoint(insertBatterySound[_batteryCount], slot.position);

            FindFirstObjectByType<ObjectInteractor>().PlaceObject(slot);
            Destroy(heldObject);
            _batteryCount++;
            
            //PuzzleManager.instance.SetPuzzleStage(2);
            
            if (_batteryCount >= 4)
            {
                Debug.Log("Activate Lamp NPC");
                //activate lil jit

            }
        }
    }
}
