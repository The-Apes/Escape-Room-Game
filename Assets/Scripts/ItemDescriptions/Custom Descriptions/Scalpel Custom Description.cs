using Managers;
using Npc;
using UnityEngine;

namespace ItemDescriptions.Custom_Descriptions
{
    public class ScalpelCustomDescription : MonoBehaviour, ICustomDescription
    {
        [SerializeField] private NpcScriptAsset scalpelScript;
        public void Describe()
        {
            if (PuzzleManager.instance.puzzleStage == 3)
            {
                ScriptManager.instance.RunScript(scalpelScript, true);
                PuzzleManager.instance.SetPuzzleStage(4);
            }
            else
            {
                if(ScriptManager.instance.CurrentScript) return; // Don't interrupt a running script
                DialogueManager.instance.SayLine("this is a Scalpel");
            }
        }
    }
}
