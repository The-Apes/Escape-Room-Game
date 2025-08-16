using System;
using Managers;
using Unity.VisualScripting;
using UnityEngine;

public class RedKeyDescription : MonoBehaviour, IItemDescription
{
    [SerializeField] private NpcScriptAsset redKeyScript;
    
    public void Describe()
    {
     int puzzleStage = PuzzleManager.instance.puzzleStage;

     switch (puzzleStage)
     {
         default:
             ScriptManager.instance.RunScript(redKeyScript);
             break;
     }
    }
}
