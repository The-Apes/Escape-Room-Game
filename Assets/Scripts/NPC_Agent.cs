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
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }
    
    void Start()
    {
        StartCoroutine(walktowardscam());
    }
    private IEnumerator walktowardscam()
    {
        yield return new WaitForSeconds(2f);
        walk = true;
        text.SetText("I'm walking towards you now!");
    }
    
    public void Speak(string line)
    {
        //TODO
        // For each line, display one character at a time same way we did bog wood
        text.SetText(line);
    }

    // Update is called once per frame
    void Update()
    {
        if (walk) agent.destination = cam.transform.position;
    }
}
