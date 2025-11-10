using Managers;
using Npc;
using UnityEngine;

namespace ItemDescriptions.Custom_Descriptions
{
    public class ComputerPasswordInspectCustomDescription : MonoBehaviour, IInspectCustomDescription
    {
        [SerializeField] private NpcScriptAsset computerPassword;

        public void Describe()
        {
            switch (PuzzleManager.instance.puzzleStage)
            {
                case 1:
                    PuzzleManager.instance.SetPuzzleStage(2);
                    ScriptManager.instance.RunScript(computerPassword);
                    break;
                default:
                    if(ScriptManager.instance.CurrentScript) return; // Don't interrupt a running script
                    DialogueManager.instance.SayLine("The Toymaker's computer password", true);
                    break;
            }
        }
    }
}
