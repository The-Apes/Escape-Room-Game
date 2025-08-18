using Managers;
using Npc;
using UnityEngine;

namespace ItemDescriptions
{
    public class ScriptDescription : MonoBehaviour, IItemDescription
    {
        [SerializeField] private NpcScriptAsset script; 
        public void Describe()
        {
            ScriptManager.instance.RunScript(script);        
        }
    }
}
