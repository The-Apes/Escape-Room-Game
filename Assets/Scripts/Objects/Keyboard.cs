using System;
using Managers;
using Npc;
using UnityEngine;

namespace Objects
{
    public class Keyboard : MonoBehaviour, IInteractable
    {
        [SerializeField] private NpcScriptAsset lockedComputerScript;
        
        [SerializeField] private GameObject greenScreen;
        [SerializeField] private GameObject redScreen;

        private void Start()
        {
            redScreen.SetActive(false);
            greenScreen.SetActive(false);
        }

        public void OnInteract(GameObject heldObject)
        {
            switch (PuzzleManager.instance.puzzleStage)
            {
                case -1:
                    return;
                case 0:
                    redScreen.SetActive(true);
                    ScriptManager.instance.RunScript(lockedComputerScript);
                    break;
            }
        }
    }
}
