using UnityEngine;
using UnityEngine.AI;

public class FollowState : State
{
    public bool waiting;
    
    private NpcAgent _npcAgent;
    private Transform _target;
    private float _followTime;
    private float _timer;

    public FollowState(NpcAgent agent, Transform target, float duration = -1f)
    {
        _target = target;
        _followTime = duration;
        _npcAgent = agent;
    }

    public State StateUpdate()
    {
        _npcAgent.agent.SetDestination(_target.position);

        if (_followTime > 0f && _npcAgent.agent.pathPending == false && _npcAgent.agent.remainingDistance <= _npcAgent.agent.stoppingDistance)
        {
            waiting = true;
            _timer += Time.deltaTime;
            if (_timer >= _followTime)
            {
                string npcLine = GenericLines.npcRoamLines[UnityEngine.Random.Range(0, GenericLines.npcCallLines.Count)];
                DialogueManager.instance.SayLine(npcLine);
                return new RoamState(_npcAgent, _npcAgent.transform);
            }
        }
        return null;
    }

}
