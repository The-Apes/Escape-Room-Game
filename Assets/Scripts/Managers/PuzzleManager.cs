using System.Collections;
using Npc;
using UnityEngine;

namespace Managers
{
    public class PuzzleManager: MonoBehaviour
    {
        public static PuzzleManager instance;
        
        public int puzzleStage; // Start with -1 to indicate no puzzle state set, but 0 for prototype

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
            StartCoroutine(TalkHelpCoroutine());
        }
        private IEnumerator TalkHelpCoroutine()
        {
            switch (puzzleStage)
            {
               case 0:
                    DialogueManager.instance.SayLine("Not sure... but there is plenty of stuff here.");
                    yield return new WaitForSeconds(3f);
                    DialogueManager.instance.SayLine("You can also bring me items and I can tell you what I know about them.");
                    FindFirstObjectByType<NpcAgent>().StopInteraction();
                    break;
                case 1:
                    DialogueManager.instance.SayLine("That object in the vent surely was hidden for a reason...");
                    break;
                case 2:
                    DialogueManager.instance.SayLine("Maybe check inside the closet for something useful?");
                    break;
                default:
                    DialogueManager.instance.SayLine("Unfortunately i'm not sure");
                    yield return null;
                    break;
            }
        }
        public void Help(){} //Npc helping on it's own
    }
}