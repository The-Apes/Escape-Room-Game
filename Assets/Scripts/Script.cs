using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Script", menuName = "Scriptable Objects/Script")]
public class Script : ScriptableObject
{
    public List <ScriptLine> scriptLines;
    public ScriptLine test;
}