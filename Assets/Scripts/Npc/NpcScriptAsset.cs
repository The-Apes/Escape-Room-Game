using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Npc
{
    [CreateAssetMenu(fileName = "NPCScript", menuName = "Scriptable Objects/NPCScript")]
    public class NpcScriptAsset : ScriptableObject
    {
        public bool interruptible;
        public bool sayOnce;
        public List <ScriptLine> scriptLines;
        [Header("Choices")]
        public NpcScriptAsset[] choices; //for every choice, there will be a script for the response
    }
}