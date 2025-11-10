using System;
using Managers;
using Npc;
using Player;
using UnityEngine;

namespace Objects
{
    public class Vent : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject key;
        [SerializeField] private NpcScriptAsset script;
        
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void OnInteract(GameObject heldObject)
        {
            print("KeyHole Interacted with");
            if(!heldObject) return;
            print("HeldObject: " + heldObject);
            print("key: " + key);
            if(PuzzleManager.instance.puzzleStage != 4) return; //only accept key at puzzle stage 4
            if(!heldObject.Equals(key)) return;
            print("Key matches, placing object in slot");
            
            gameObject.tag = "canPickUp";
            _rigidbody.isKinematic = false; //make the vent fall down
            ScriptManager.instance.RunScript(script);
            FindFirstObjectByType<Hands>().Grab();
            PuzzleManager.instance.SetPuzzleStage(5);
        }
    }
}
