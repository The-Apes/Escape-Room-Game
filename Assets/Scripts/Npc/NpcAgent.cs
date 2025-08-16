using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NpcAgent : MonoBehaviour
{
    public TextMeshPro text;

   public NavMeshAgent agent;
    private Camera _cam;
    private bool _walk;
    private bool _talking;

    private enum States
    {
        Roam,
        Follow,
        GoTo,
        Script
    }
    
    States _currentState = States.Roam;
    public State ActiveState;
    private void SetTalking(bool value)
    {
        _talking = value;
        if(ScriptManager.instance){ScriptManager.instance.NpcTalking = value;}
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _cam = Camera.main;
        ActiveState = new RoamState(this, transform);
    }
    private void Start()
    {
        text.SetText("");
    }
    public void Speak(string line, float duration)
    {
        StartCoroutine(SpeakCoroutine(line, duration));
    }
    private IEnumerator SpeakCoroutine(string line, float duration)
    {
        //TODO
        // For each line, display one character at a time same way we did bog wood
        //SetTalking(true); don't wanna break nothing
        text.SetText(line);
        yield return new WaitForSeconds(duration);
        text.SetText("");
        SetTalking(false);
    }

    public void Action(string action,string parameter = null)
    {
        switch (action)
        {
            case "go to player":
                agent.destination = _cam.transform.position;
                break;
            case "go to object":
                agent.destination = ScriptManager.instance.CurrentLine.customObject.transform.position;
                break;
            case "go to location":
                agent.destination = ScriptManager.instance.CurrentLine.location;
                break;
        }
    }

    public void input()
    {
        switch (_currentState)
        {
            case States.Roam:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var newState = ActiveState.StateUpdate();
        if (newState != null)
        {
            ChangeState(newState);
        }
    }

    private void ChangeState(State newState)
    {
        ActiveState.exitState();
        ActiveState = newState;
        ActiveState.enterState();
    }
}
