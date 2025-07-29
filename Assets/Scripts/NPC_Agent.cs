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
    
    private NavMeshAgent agent;
    private Camera cam;
    public TextMeshPro text;

    private bool walk;

    private bool _talking;

    private void SetTalking(bool value)
    {
        _talking = value;
        if(ScriptManager.instance){ScriptManager.instance.NpcTalking = value;}
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
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
                agent.destination = cam.transform.position;
                break;
            case "go to object":
                agent.destination = ScriptManager.instance.CurrentLine.customObject.transform.position;
                break;
            case "go to location":
                agent.destination = ScriptManager.instance.CurrentLine.location;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (walk) agent.destination = cam.transform.position;
    }
}
