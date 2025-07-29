using System;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager instance;
    
    [SerializeField] private Script introduction;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void Start()
    {
        RunScript(introduction);
    }

    public void RunScript(Script script)
    {
        foreach (ScriptLine line in script.scriptLines)
        {
            //Tells the dialogue manager to handle the Line
            DialogueManager.Instance.SayLine(line.text);
            
            // Does the other actions per the script line
            
            // Waits for the condition for the dialogue to be met to continue
        }
    }
}
