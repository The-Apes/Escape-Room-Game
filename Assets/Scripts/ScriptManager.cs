using System;
using System.Collections;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager instance;
    
    [SerializeField] private Script introduction;

    private bool _nextLine;
    
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
        StartCoroutine(RunScript(introduction));
    }

    public IEnumerator RunScript(Script script)
    {
        foreach (ScriptLine line in script.scriptLines)
        {
            _nextLine = false;
            //Tells the dialogue manager to handle the Line
            DialogueManager.Instance.SayLine(line.text);
            
            // Does the other actions per the script line
            
            // Waits for the condition for the dialogue to be met to continue
            String command = line.condition.Substring(0, line.condition.IndexOf(':')).ToLower();
            String parameters = line.condition.Substring(line.condition.IndexOf(':') + 1).Trim().ToLower();
            switch (command)
            {
                case "wait": 
                    //Waits for the specified time
                    if (float.TryParse(parameters, out float waitTime))
                    {
                        StartCoroutine(WaitForSeconds(waitTime));
                    }
                    else
                    {
                        Debug.LogError("Invalid Parameter for 'wait' condition: " + parameters);
                    }
                    break;
                default:
                    Debug.LogError("Unknown command:"+command);
                    break;
            }

            while (!_nextLine)
            {
                yield return null;
            }
        }
    }

    private IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _nextLine = true;
    }
}
