using System.Collections;
using Npc;
using UnityEngine;

namespace Managers
{
    public class PuzzleManager: MonoBehaviour
    {
        public static PuzzleManager instance;
        
        public int puzzleStage; 
        
        [SerializeField] private NpcScriptAsset puzzleStage0Help;
        [SerializeField] private NpcScriptAsset prototypeEnd; // Remove

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void SetPuzzleStage(int stage)
        {
            if (!(stage > puzzleStage)) return; // Ensure state is greater than current state
            puzzleStage = stage;
            Debug.Log("Puzzle state set to: " + puzzleStage);
        }

        public void TalkHelp()
        {
            switch (puzzleStage)
            {
                case 0:
                    ScriptManager.instance.RunScript(puzzleStage0Help);
                    //do I need? FindFirstObjectByType<NpcAgent>().StopInteraction();
                    break;
                case 1:
                    DialogueManager.instance.SayLine("That object in the vent surely was hidden for a reason...");
                    FindFirstObjectByType<NpcAgent>().StopInteraction();
                    break;
                case 2:
                    DialogueManager.instance.SayLine("Maybe check inside the closet for something useful?");
                    FindFirstObjectByType<NpcAgent>().StopInteraction();
                    break;
                case 3: 
                    ScriptManager.instance.RunScript(prototypeEnd);  //make a script list with disctionaries that can be acsessed with as string key?
                    break;
                default:
                    DialogueManager.instance.SayLine("Unfortunately i'm not sure");
                    FindFirstObjectByType<NpcAgent>().StopInteraction();
                    break;
            }
            //make stop interaction line run after switch statement? or keep it on a per case basis?
        }
        public void Help(){} //Npc helping on it's own
    }
}