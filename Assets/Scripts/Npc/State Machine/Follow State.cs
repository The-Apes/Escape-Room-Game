using Managers;
using UnityEngine;

namespace Npc.State_Machine
{
    public class FollowState : IState
    {
        public bool Waiting;
    
        private readonly NpcAgent _npcAgent;
        private readonly Transform _target;
        private readonly float _followTime;
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
                Waiting = true;
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
}
