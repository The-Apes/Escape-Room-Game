using System;
using System.Collections;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager instance;
    
    [SerializeField] private NpcScriptAsset _introduction;
    private NpcAgent _npcAgent;

    private bool _nextLine;
    [NonSerialized] public bool NpcTalking;
    [NonSerialized] public ScriptLine CurrentLine;
    [NonSerialized] public NpcScriptAsset CurrentScript;
    
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
        _npcAgent = FindFirstObjectByType<NpcAgent>();
        if(_introduction) StartCoroutine(RunScript(_introduction));
    }

    public IEnumerator RunScript(NpcScriptAsset script)
    {
        CurrentScript = script;
        foreach (ScriptLine line in script.scriptLines)
        {
            CurrentLine = line;
            _nextLine = false;
            //Tells the dialogue manager to handle the Line
            DialogueManager.instance.SayLine(line.text, line.player);
            
            // Does the other actions per the script line
           
            foreach (var actionLine in line.actions)
            {
                String action;
                if (actionLine.Contains(':'))
                {
                     action = actionLine.Substring(0, actionLine.IndexOf(':')).ToLower();
                     String actionParam = actionLine.Substring(actionLine.IndexOf(':') + 1).Trim().ToLower();
                     _npcAgent.Action(action, actionParam);
                }
                else
                {
                    action = actionLine.Trim().ToLower();
                    //actionParam = String.Empty;
                    _npcAgent.Action(action);
                }
                
            }
            
            // Waits for the condition for the dialogue to be met to continue
            String condition = line.continueCondition.Substring(0, line.continueCondition.IndexOf(':')).ToLower();
            String parameters = line.continueCondition.Substring(line.continueCondition.IndexOf(':') + 1).Trim().ToLower();
            switch (condition)
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
                case "question":
                    ChoiceManager.instance.Ask(parameters);
                    break;
                case "player distance less than":
                    if (float.TryParse(parameters, out float distance))
                    {
                        StartCoroutine(PlayerDistanceLessThan(distance));
                    }
                    else
                    {
                        Debug.LogError("Invalid Parameter for 'player distance less than' condition: " + parameters);
                    }
                    break;
                case "distance to object":
                    break;
                case "has item":
                    break;
                case "player looking at npc":
                    break;
                case "player looking at object":
                    break;
                default:
                    Debug.LogError("Unknown command:"+condition);
                    Debug.LogWarning("Waiting for 3 seconds instead");
                    StartCoroutine(WaitForSeconds(3));
                    break;
            }

            while (!_nextLine && !NpcTalking)
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
    private IEnumerator PlayerDistanceLessThan(float distance)
    {
        GameObject player = FindFirstObjectByType<PlayerMovement>().gameObject;
        while (Vector3.Distance(_npcAgent.transform.position, player.transform.position) > distance)
        {
            yield return null;
        }
        _nextLine = true;
    }
}
