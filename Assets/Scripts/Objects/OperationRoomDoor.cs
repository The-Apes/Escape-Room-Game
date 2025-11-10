using Managers;
using Npc;
using Player;
using UnityEngine;

namespace Objects
{
    public class KeypadInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject key;
        [SerializeField] private NpcScriptAsset DoorUnlcok;
    

        public void OnInteract(GameObject heldObject)
        {
            if(!heldObject) return;
            if(!heldObject.Equals(key)) return;
            transform.rotation = Quaternion.Euler(-90, 0, 90);
            PuzzleManager.instance.SetPuzzleStage(6);
            FindFirstObjectByType<Hands>().Grab();

            //revealNavmesh
            ScriptManager.instance.RunScript(DoorUnlcok);
        }
    }
}
