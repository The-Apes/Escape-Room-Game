using Managers;
using Npc;
using UnityEngine;

namespace ItemDescriptions
{
    public class InspectScriptDescription : MonoBehaviour, IItemDescription
    {
        [SerializeField] private NpcScriptAsset script; 
        public void Describe()
        {
            ScriptManager.instance.RunScript(script);    
        }
    }
}
