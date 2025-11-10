using Managers;
using Npc;
using UnityEngine;

namespace ItemDescriptions.Custom_Descriptions
{
    public class BatteryCustomDescription : MonoBehaviour, ICustomDescription
    {
        [SerializeField] private NpcScriptAsset scriptToPlay;
        public void Describe()
        {
            if (PuzzleManager.instance.puzzleStage == 1)
            {
                ScriptManager.instance.RunScript(scriptToPlay, true);
            }
            else
            {
                if(ScriptManager.instance.CurrentScript) return; // Don't interrupt a running script
                DialogueManager.instance.SayLine("this is a battery");
            }
        }
    }
}
