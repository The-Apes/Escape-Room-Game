using Npc.State_Machine;
using UnityEngine;
using UnityEngine.AI;

public class FollowState : IState
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

    public IState StateUpdate()
    {
        _npcAgent.Agent.SetDestination(_target.position);

        if (_followTime > 0f && _npcAgent.Agent.pathPending == false && _npcAgent.Agent.remainingDistance <= _npcAgent.Agent.stoppingDistance)
        {
            waiting = true;
            _timer += Time.deltaTime;
            if (_timer >= _followTime && !_npcAgent.Busy)
            {
                DialogueManager.instance.SayLine(GenericLines.GetRandomLine("Npc Roam"));
                return new RoamState(_npcAgent, _npcAgent.transform);
            }
        }
        return null;
    }

}
