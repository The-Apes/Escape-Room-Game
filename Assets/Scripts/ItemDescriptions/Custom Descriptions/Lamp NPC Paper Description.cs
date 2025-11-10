using Managers;
using UnityEngine;

namespace ItemDescriptions.Custom_Descriptions
{
    public class LampNpcPaperDescription : MonoBehaviour, IInspectCustomDescription
    {
        public void Describe()
        {
            switch (PuzzleManager.instance.puzzleStage)
            {
                case -1:
                    if(ScriptManager.instance.CurrentScript) return; // Don't interrupt a running script
                    DialogueManager.instance.SayLine("I need 4 batteries.. and this this will turn on?", true);
                    break;
                default:
                    if(ScriptManager.instance.CurrentScript) return; // Don't interrupt a running script
                    DialogueManager.instance.SayLine("So this thing also is meant for children??", true);
                    break;
            }
        }
    }
}
