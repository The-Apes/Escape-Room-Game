using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcInteraction : MonoBehaviour
{
    private NpcAgent _npcAgent;

    private void Awake()
    {
        _npcAgent = FindFirstObjectByType<NpcAgent>();
        if (_npcAgent == null)
        {
            Debug.LogError("NpcAgent component not found on the GameObject.");
        }
    }

    public void CallNpcInput(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        StartCoroutine(CallNpcCoroutine());
    }
    public IEnumerator CallNpcCoroutine()
    {
        string line = GenericLines.callLines[UnityEngine.Random.Range(0, GenericLines.callLines.Count)];
        DialogueManager.instance.SayLine(line,true);
        yield return new WaitForSeconds(1f);
        
        // Check if the NPC is already following the player
        string npcLine;
        if (_npcAgent.ActiveState is FollowState)
        {
            Debug.Log("NPC is already following the player.");
            npcLine = GenericLines.redundantNpcCallLines[UnityEngine.Random.Range(0, GenericLines.redundantNpcCallLines.Count)];
            DialogueManager.instance.SayLine(npcLine);

        }
        else
        {
             npcLine = GenericLines.npcCallLines[UnityEngine.Random.Range(0, GenericLines.npcCallLines.Count)];
             FollowState followState = (FollowState)(_npcAgent.ActiveState = new FollowState(_npcAgent, transform, 5f));
             DialogueManager.instance.SayLine(npcLine);

             while (!followState.waiting)
             {
                    yield return null; // Wait until the NPC starts Waiting
             }
             npcLine = GenericLines.npcArrivalLines[UnityEngine.Random.Range(0, GenericLines.npcArrivalLines.Count)];
             DialogueManager.instance.SayLine(npcLine);

        }
    }
 
    

}
