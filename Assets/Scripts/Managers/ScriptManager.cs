using System;
using System.Collections;
using Npc;
using Player;
using UnityEngine;

namespace Managers
{
    public class ScriptManager : MonoBehaviour
    {
        public static ScriptManager instance;
    
        [SerializeField] private NpcScriptAsset introduction;
        private NpcAgent _npcAgent;

        private bool _nextLine;
        [NonSerialized] public bool NpcTalking;
        [NonSerialized] public ScriptLine CurrentLine;
        [NonSerialized] public NpcScriptAsset CurrentScript;
        [NonSerialized] public NpcScriptAsset MostRecentScript;
    
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
            _npcAgent = FindFirstObjectByType<NpcAgent>();
        }
    
        public void Start()
        {
            //if(introduction) StartCoroutine(RunScriptCoroutine(introduction));
        }

        public void RunScript(NpcScriptAsset script, bool force = false)
        {
            // If the script is marked as completed, do nothing
            if (PlayerFlagsManager.instance.CompletedScripts.Contains(script.name))
            {
                Debug.Log("Script already completed: " + script.name);
                return;
            }

            // If a script is already running and the new script is not interruptible, do nothing
            print(force);
            if (CurrentScript && !script.interruptible && !force)     
            {
                Debug.Log("Script already running and new script not interruptible: " + CurrentScript.name); 
                return;
            }
            if (!script)
            {
                Debug.Log("No Script");
                _npcAgent.ScriptEnd();
                return;
            }
            StopAllCoroutines();
            StartCoroutine(RunScriptCoroutine(script));
        }

        private IEnumerator RunScriptCoroutine(NpcScriptAsset script)
        {
            OnScriptStart();
            CurrentScript = script;
            MostRecentScript = script;
            if (_npcAgent) _npcAgent.ScriptStart();
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
                String condition = line.continueCondition.Contains(':') ? line.continueCondition.Substring(0, line.continueCondition.IndexOf(':')).ToLower() : line.continueCondition.Trim().ToLower();
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
                    case "wait extra": 
                        //Waits for the specified time
                        if (float.TryParse(parameters, out float extraWaitTime))
                        {
                            StartCoroutine(WaitForSeconds(DialogueManager.instance.dialogueWaitTime+extraWaitTime));
                        }
                        else
                        {
                            Debug.LogError("Invalid Parameter for 'wait' condition: " + parameters);
                        }
                        break;
                    case "question":
                        ChoiceManager.instance.Ask(parameters);
                        break;
                    case "destination":
                        StartCoroutine(WaitUntilDestination());
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
                        StartCoroutine(WaitForSeconds(DialogueManager.instance.dialogueWaitTime));
                        break;
                }

                while (!_nextLine && !NpcTalking)
                {
                    yield return null;
                }
            }
            OnScriptEnd();
            CurrentScript = null;
            _npcAgent.ScriptEnd();
        }
        private void OnScriptStart()
        {
            // Any setup needed at the start of a script
        }
        private void OnScriptEnd()
        {
            Debug.Log("Script Ended: " + CurrentScript.name);
            
            // Mark script as completed if sayOnce is true
            if (CurrentScript.sayOnce)
            {
                PlayerFlagsManager.instance.CompletedScripts.Add(CurrentScript.name);
            }
            switch (CurrentScript.name)
            {
                case "Start":
                    TutorialManager.instance.InteractTutorial();
                    break;
                case "Lamp Wakeup":
                    TutorialManager.instance.NpcTutorial();
                    PuzzleManager.instance.SetPuzzleStage(0);
                    break;
                case "Locked Computer":
                    TutorialManager.instance.NpcGiveTutorial();
                    PuzzleManager.instance.SetPuzzleStage(1);
                    break;
                case "Battery":
                    FindFirstObjectByType<Torch>().ActivateTorch();
                    break;
                    
            }
        }
        public void NextLine()
        {
            _nextLine = true;
        }

        private IEnumerator WaitForSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _nextLine = true;
        }

        private IEnumerator WaitUntilDestination()
        {
            while (!(_npcAgent.Agent.pathPending == false && _npcAgent.Agent.remainingDistance <= _npcAgent.Agent.stoppingDistance))
            {
                yield return null;
            }
            _nextLine = true;
        }
        private IEnumerator PlayerDistanceLessThan(float distance)
        {
            GameObject player = FindFirstObjectByType<FpController>().gameObject;
            while (Vector3.Distance(_npcAgent.transform.position, player.transform.position) > distance)
            {
                yield return null;
            }
            _nextLine = true;
        }
    }
}
