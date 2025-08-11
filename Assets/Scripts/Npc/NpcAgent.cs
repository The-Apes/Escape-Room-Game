using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NpcAgent : MonoBehaviour
{
    private enum State
    {
        Roam,
        Follow,
        GoTo,
    }
    
    private NavMeshAgent _agent;
    private Camera _cam;
    public TextMeshPro text;

    private bool _walk;

    private bool _talking;

    private void SetTalking(bool value)
    {
        _talking = value;
        if(ScriptManager.instance){ScriptManager.instance.NpcTalking = value;}
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _cam = Camera.main;
    }
    
    public void Speak(string line)
    {
        //TODO
        // For each line, display one character at a time same way we did bog wood
        text.SetText(line);
        SetTalking(false);
    }

    public void Action(string action,string parameter = null)
    {
        switch (action)
        {
            case "go to player":
                _agent.destination = _cam.transform.position;
                break;
            case "go to object":
                _agent.destination = ScriptManager.instance.CurrentLine.customObject.transform.position;
                break;
            case "go to location":
                _agent.destination = ScriptManager.instance.CurrentLine.location;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_walk) _agent.destination = _cam.transform.position;
    }
}
