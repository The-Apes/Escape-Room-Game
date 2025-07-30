using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Script", menuName = "Scriptable Objects/Script")]
public class Script : ScriptableObject
{
    public List <ScriptLine> scriptLines;
    [Header("Choices")]
    public Script[] choices; //for every choice, there will be a script for the response
}