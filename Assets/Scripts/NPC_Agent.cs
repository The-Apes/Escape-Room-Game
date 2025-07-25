using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NpcAgent : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        if (walk) agent.destination = cam.transform.position;
    }
}
