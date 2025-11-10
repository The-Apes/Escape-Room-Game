using System;
using Managers;
using Npc;
using UnityEngine;

namespace Objects
{
    public class Keyboard : MonoBehaviour, IInteractable
    {
        [SerializeField] private NpcScriptAsset lockedComputerScript;
        [SerializeField] private NpcScriptAsset ventScript;
        
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
                default:
                    return;
                case 0:
                    redScreen.SetActive(true);
                    ScriptManager.instance.RunScript(lockedComputerScript);
                    PuzzleManager.instance.SetPuzzleStage(1);
                    break;
                case 2:
                    PuzzleManager.instance.SetPuzzleStage(3);
                    redScreen.SetActive(false);
                    greenScreen.SetActive(true);
                    ScriptManager.instance.RunScript(ventScript);
                    break;
            }
        }
    }
}
