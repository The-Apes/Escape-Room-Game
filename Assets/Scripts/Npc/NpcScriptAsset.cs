using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCScript", menuName = "Scriptable Objects/NPCScript")]
public class NpcScriptAsset : ScriptableObject
{
    public List <ScriptLine> scriptLines;
    [Header("Choices")]
    public NpcScriptAsset[] choices; //for every choice, there will be a script for the response
}