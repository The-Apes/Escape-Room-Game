using Managers;
using Npc;
using UnityEngine;

namespace ItemDescriptions.Custom_Descriptions
{
    public class SyringeCustomDescription : MonoBehaviour, ICustomDescription
    {
        public void Describe()
        {
            if (PuzzleManager.instance.puzzleStage == 3)
            {
                if(ScriptManager.instance.CurrentScript) return; // Don't interrupt a running script
                DialogueManager.instance.SayLine("This wont do!");
            }
            else
            {
                if(ScriptManager.instance.CurrentScript) return; // Don't interrupt a running script
                DialogueManager.instance.SayLine("this is a syringe of sorts");
            }
        }
    }
}
