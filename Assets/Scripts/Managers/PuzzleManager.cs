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
                    DialogueManager.instance.SayLine("The Keys in the door but it's still not working...");
                    yield return new WaitForSeconds(1f);
                    DialogueManager.instance.SayLine("I blame Phiwe, fat neek");
                    break;
                case 2:
                    Debug.Log("Puzzle Stage 2: Offering help for the second puzzle.");
                    // Add specific help logic here
                    yield return new WaitForSeconds(1f);
                    break;
                default:
                    Debug.Log("No specific help available for this puzzle stage.");
                    yield return null;
                    break;
            }
        }
        public void Help(){} //Npc helping on it's own
    }
}